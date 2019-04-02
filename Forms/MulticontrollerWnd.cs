﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;

namespace TTMulti.Forms
{
    /// <summary>
    /// The main window. This window captures all input sent to the window and child controls by 
    /// implementing IMessageFilter and overriding ProcessCmdKey(). All input is sent to the Multicontroller class.
    /// A low-level keyboard hook is also used to listen for the mode key when a Toontown window is active.
    /// </summary>
    internal partial class MulticontrollerWnd : Form, IMessageFilter
    {
        // Low-level keyboard hook related fields
        IntPtr _llHookID = IntPtr.Zero;
        Win32.HookProc llKeyboardProc = null;

        /// <summary>
        /// This flag is used to ignore input while a dialog is open.
        /// </summary>
        bool ignoreMessages = false;

        /// <summary>
        /// The thread used to work around activation issues.
        /// </summary>
        Thread activationThread = null;

        Multicontroller controller;

        internal MulticontrollerWnd()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.icon;
        }
        
        IntPtr LLKeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                Win32.WM msg = (Win32.WM)wParam;

                if (msg == Win32.WM.KEYDOWN || msg == Win32.WM.KEYUP) 
                {
                    Keys vkCode = (Keys)Marshal.ReadInt32(lParam);

                    if (vkCode == (Keys)Properties.Settings.Default.modeKeyCode)
                    {
                        bool ret = controller.ProcessKey(vkCode, (uint)msg);

                        if (ret)
                        {
                            return (IntPtr)1;
                        }
                    }
                }
            }

            return Win32.CallNextHookEx(_llHookID, nCode, wParam, lParam);
        }

        /// <summary>
        /// Activates the window.
        /// Works around an issue where sometimes calling Activate() doesn't activate the window.
        /// If calling Activate() doesn't work, this makes the window topmost and fakes a mouse event.
        /// </summary>
        internal void TryActivate()
        {
            if (activationThread == null || activationThread.ThreadState != System.Threading.ThreadState.Running)
            {
                activationThread = new Thread(activationThreadFunc) { IsBackground = true };
                activationThread.Start();
            }
        }

        private void activationThreadFunc()
        {
            IntPtr hWnd = IntPtr.Zero;
            this.InvokeIfRequired(() => hWnd = this.Handle);

            Stopwatch sw = Stopwatch.StartNew();

            do
            {
                // This check was put in to prevent exceptions when the window is closing.
                if (this.IsDisposed)
                {
                    return;
                }

                // First try calling Activate() for some time
                if (sw.ElapsedMilliseconds < 100)
                {
                    this.InvokeIfRequired(() => this.Activate());
                }
                // If that doesn't work, fake a mouse event to activate the window
                else
                {
                    this.InvokeIfRequired(() => this.TopMost = true);

                    int x = this.DisplayRectangle.Width / 2 + this.Location.X;
                    int y = this.Location.Y + SystemInformation.CaptionHeight / 2;

                    var virtualScreen = SystemInformation.VirtualScreen;
                    x = (x - virtualScreen.Left) * 65536 / virtualScreen.Width;
                    y = (y - virtualScreen.Top) * 65536 / virtualScreen.Height;

                    int oldX = (int)Math.Ceiling((MousePosition.X - virtualScreen.Left) * 65536.0 / virtualScreen.Width),
                        oldY = (int)Math.Ceiling((MousePosition.Y - virtualScreen.Top) * 65536.0 / virtualScreen.Height);

                    var pInputs = new[]{
                        new Win32.INPUT() {
                            type = 0, //mouse event
                            U = new Win32.InputUnion() {
                                mi = new Win32.MOUSEINPUT() {
                                    dx = x,
                                    dy = y,
                                    dwFlags = Win32.MOUSEEVENTF.ABSOLUTE | Win32.MOUSEEVENTF.VIRTUALDESK | Win32.MOUSEEVENTF.MOVE | Win32.MOUSEEVENTF.LEFTDOWN
                                }
                            }
                        },
                        new Win32.INPUT() {
                            type = 0, //mouse event
                            U = new Win32.InputUnion() {
                                mi = new Win32.MOUSEINPUT() {
                                    dx = 0,
                                    dy = 0,
                                    dwFlags = Win32.MOUSEEVENTF.LEFTUP
                                }
                            }
                        },
                        new Win32.INPUT() {
                            type = 0, //mouse event
                            U = new Win32.InputUnion() {
                                mi = new Win32.MOUSEINPUT() {
                                    dx = oldX,
                                    dy = oldY,
                                    dwFlags = Win32.MOUSEEVENTF.ABSOLUTE | Win32.MOUSEEVENTF.VIRTUALDESK | Win32.MOUSEEVENTF.MOVE
                                }
                            }
                        }
                    };

                    Win32.SendInput((uint)pInputs.Length, pInputs, Win32.INPUT.Size);
                }

                Thread.Sleep(10);

                if (Win32.GetForegroundWindow() == hWnd)
                {
                    this.InvokeIfRequired(() => this.TopMost = Properties.Settings.Default.onTopWhenInactive);
                    break;
                }
            } while (sw.Elapsed.TotalSeconds < 5);

            sw.Stop();
        }

        /// <summary>
        /// Updates the window selectors and group status.
        /// This should be called when the current group or window selection changes.
        /// </summary>
        internal void UpdateWindowStatus()
        {
            leftToonCrosshair.SelectedWindowHandle = controller.LeftController.TTWindowHandle;
            rightToonCrosshair.SelectedWindowHandle = controller.RightController.TTWindowHandle;

            leftStatusLbl.Text = "Group " + (controller.CurrentGroupIndex + 1) + " active.";
            rightStatusLbl.Text = controller.ControllerGroups.Count + " groups.";

            if (!statusStrip1.Visible && controller.ControllerGroups.Count > 1)
            {
                statusStrip1.Visible = true;
                this.Padding = new Padding(this.Padding.Left, this.Padding.Top, this.Padding.Right, this.Padding.Bottom + statusStrip1.Height);
            }
            else if (statusStrip1.Visible && controller.ControllerGroups.Count == 1)
            {
                this.Padding = new Padding(this.Padding.Left, this.Padding.Top, this.Padding.Right, this.Padding.Bottom - statusStrip1.Height);
                statusStrip1.Visible = false;
            }
        }

        /// <summary>
        /// Overrides keys that usually perform other functions like tab, arrow keys, etc. so that
        /// we can use them for control. After getting intercepted, they are caught by the message filter.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns>
        /// Returns true when the key should be intercepted so they don't perform their usual function.
        /// </returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Tab:
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                case Keys.Alt:
                    return true;
                default:
                    break;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// IMessageFilter function implementation. This captures all keys sent to the window, including ones
        /// that are sent directly to child controls, and sends them to the multicontroller.
        /// </summary>
        /// <param name="m"></param>
        /// <returns>
        /// Returns true when the key should be stopped from getting to its destination.
        /// </returns>
        public bool PreFilterMessage(ref Message m)
        {
            if (ignoreMessages)
            {
                return false;
            }

            bool ret = false;
            
            var msg = (Win32.WM)m.Msg;
            
            if (msg == Win32.WM.KEYDOWN || msg == Win32.WM.KEYUP || msg == Win32.WM.SYSKEYDOWN || msg == Win32.WM.SYSKEYUP || msg == Win32.WM.SYSCOMMAND)
            {
                var key = (Keys)m.WParam.ToInt32();
                var settings = Properties.Settings.Default;

                ret = controller.ProcessKey(key, (uint)m.Msg, m.LParam);
            }

            return ret;
        }
        
        internal void SaveWindowPosition()
        {
            Properties.Settings.Default.lastLocation = this.Location;
            Properties.Settings.Default.Save();
        }
        
        private void ReloadOptions()
        {
            this.TopMost = Properties.Settings.Default.onTopWhenInactive;
            wndGroup.Visible = !Properties.Settings.Default.compactUI;
            controller.UpdateKeys();
        }

        private void MulticontrollerWnd_Load(object sender, EventArgs e)
        {
            controller = Multicontroller.Instance;
            controller.Init();

            controller.ModeChanged += Controller_ModeChanged;
            controller.GroupsChanged += Controller_GroupsChanged;
            controller.ShouldActivate += Controller_ShouldActivate;

            // Removes the extra padding on the right side of the status strip.
            // Apparently this is "not relevant for this class" but still has an effect.
            statusStrip1.Padding = new Padding(statusStrip1.Padding.Left, statusStrip1.Padding.Top, statusStrip1.Padding.Left, statusStrip1.Padding.Bottom);

            // Set up the IMessageFilter so we receive all messages for child controls
            Application.AddMessageFilter(this);

            // Set up the low-level keyboard hook used for activating the multicontroller 
            // from any selected Toontown window.
            llKeyboardProc = new Win32.HookProc(LLKeyboardHookCallback);

            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                _llHookID = Win32.SetWindowsHookEx(Win32.WH_KEYBOARD_LL, llKeyboardProc, Win32.GetModuleHandle(curModule.ModuleName), 0);
            }

            // Restore the saved position of the window, making sure that it's not offscreen
            if (Properties.Settings.Default.lastLocation != Point.Empty)
            {
                var location = Properties.Settings.Default.lastLocation;
                var isNotOffScreen = false;

                foreach (var screen in Screen.AllScreens)
                {
                    if (screen.Bounds.Contains(location))
                    {
                        isNotOffScreen = true;
                        break;
                    }
                }

                if (isNotOffScreen)
                {
                    this.Location = Properties.Settings.Default.lastLocation;
                }
            }

            ReloadOptions();

            // Multicontroller could have loaded groups
            UpdateWindowStatus();
        }
        
        private void MainWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                activationThread.Abort();
            }
            catch { }

            Win32.UnhookWindowsHookEx(_llHookID);
            SaveWindowPosition();
        }

        private void Controller_GroupsChanged(object sender, EventArgs e)
        {
            this.UpdateWindowStatus();
        }

        private void Controller_ShouldActivate(object sender, EventArgs e)
        {
            this.TryActivate();
        }

        private void Controller_ModeChanged(object sender, EventArgs e)
        {
            this.InvokeIfRequired(() =>
            {
                switch (controller.CurrentMode)
                {
                    case Multicontroller.ControllerMode.Multi:
                        multiModeRadio.Checked = true;
                        break;
                    case Multicontroller.ControllerMode.Mirror:
                        mirrorModeRadio.Checked = true;
                        break;
                }
            });
        }

        private void optionsBtn_Click(object sender, EventArgs e)
        {
            OptionsDlg optionsDlg = new OptionsDlg();

            ignoreMessages = true;

            if (optionsDlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                ReloadOptions();
            }

            ignoreMessages = false;
        }

        private void windowGroupsBtn_Click(object sender, EventArgs e)
        {
            ignoreMessages = true;
            new WindowGroupsForm().ShowDialog(this);
            ignoreMessages = false;

            UpdateWindowStatus();
        }

        private void leftToonCrosshair_WindowSelected(object sender, IntPtr handle)
        {
            Multicontroller.Instance.LeftController.TTWindowHandle = handle;
        }

        private void rightToonCrosshair_WindowSelected(object sender, IntPtr handle)
        {
            Multicontroller.Instance.RightController.TTWindowHandle = handle;
        }

        private void multiModeRadio_Click(object sender, EventArgs e)
        {
            controller.CurrentMode = Multicontroller.ControllerMode.Multi;
        }

        private void mirrorModeRadio_Clicked(object sender, EventArgs e)
        {
            controller.CurrentMode = Multicontroller.ControllerMode.Mirror;
        }

        private void MulticontrollerWnd_Activated(object sender, EventArgs e)
        {
            controller.IsActive = true;
        }

        private void MulticontrollerWnd_Deactivate(object sender, EventArgs e)
        {
            controller.IsActive = false;
        }
    }
}
