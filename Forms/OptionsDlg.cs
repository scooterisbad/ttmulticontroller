﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Deployment;
using System.Deployment.Application;
using TTMulti.Controls;

namespace TTMulti.Forms
{
    public partial class OptionsDlg : Form
    {
        private bool loaded = false;

        public OptionsDlg()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.icon;

            toolTip1.SetToolTip(checkBox2, "If checked, the Multicontroller window will stay on top of everything else. Otherwise, it will go to the background when it's deactivated by clicking on another window.");
            toolTip1.SetToolTip(checkBox3, "If checked, some of the UI elements will be hidden to make the Multicontroller window smaller.");

            if (!ApplicationDeployment.IsNetworkDeployed)
            {
                checkUpdateBtn.Visible = false;
            }
        }

        private void OptionsDlg_Load(object sender, EventArgs e)
        {
            bindingsMultiPicker.KeyMappings = Properties.SerializedSettings.Default.Bindings;
            leftMultiPicker.KeyMappings = Properties.SerializedSettings.Default.LeftKeys;
            rightMultiPicker.KeyMappings = Properties.SerializedSettings.Default.RightKeys;

            loaded = true;
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            Properties.SerializedSettings.Default.Bindings = bindingsMultiPicker.KeyMappings;
            Properties.SerializedSettings.Default.LeftKeys = leftMultiPicker.KeyMappings;
            Properties.SerializedSettings.Default.RightKeys = rightMultiPicker.KeyMappings;
            
            Properties.Settings.Default.Save();
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            DialogResult = DialogResult.Cancel;
        }

        private void aboutBtn_Click(object sender, EventArgs e)
        {
            new AboutWnd().ShowDialog(this);
        }

        // https://docs.microsoft.com/en-us/visualstudio/deployment/how-to-check-for-application-updates-programmatically-using-the-clickonce-deployment-api?view=vs-2015
        private void checkUpdateBtn_Click(object sender, EventArgs e)
        {
            UpdateCheckInfo info = null;

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;

                try
                {
                    info = ad.CheckForDetailedUpdate();
                }
                catch (DeploymentDownloadException dde)
                {
                    MessageBox.Show("The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: " + dde.Message);
                    return;
                }
                catch (InvalidDeploymentException ide)
                {
                    MessageBox.Show("Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: " + ide.Message);
                    return;
                }
                catch (InvalidOperationException ioe)
                {
                    MessageBox.Show("This application cannot be updated. It is likely not a ClickOnce application. Error: " + ioe.Message);
                    return;
                }

                if (info.UpdateAvailable)
                {
                    Boolean doUpdate = true;

                    if (!info.IsUpdateRequired)
                    {
                        DialogResult dr = MessageBox.Show("An update is available. Would you like to update the application now?", "Update Available", MessageBoxButtons.OKCancel);
                        if (!(DialogResult.OK == dr))
                        {
                            doUpdate = false;
                        }
                    }
                    else
                    {
                        // Display a message that the app MUST reboot. Display the minimum required version.
                        MessageBox.Show("This application has detected a mandatory update from your current " +
                            "version to version " + info.MinimumRequiredVersion.ToString() +
                            ". The application will now install the update and restart.",
                            "Update Available", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }

                    if (doUpdate)
                    {
                        try
                        {
                            ad.Update();
                            MessageBox.Show("The application has been upgraded, and will now restart.");
                            Application.Restart();
                        }
                        catch (DeploymentDownloadException dde)
                        {
                            MessageBox.Show("Cannot install the latest version of the application. \n\nPlease check your network connection, or try again later. Error: " + dde);
                            return;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("There are no updates available at the moment.", "No Update Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("This application can't be updated. Try re-installing it from the homepage.", "Cannot Be Updated", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void addBindingBtn_Click(object sender, EventArgs e)
        {
            bindingsMultiPicker.AddMapping("Custom", Keys.None, false);
        }

        /**
         * This is the best way I could think of to keep the multipickers in sync. 
         * I tried doing a rebuild every time on the left and right multipickers but
         * that just caused issues with the TableLayoutPanel layouts.
         */

        private void bindingsMultiPicker_KeyMappingAdded(object sender, KeyMapping e)
        {
            // Don't do anything if the window hasn't loaded yet. 
            // Otherwise, events received from the bindings picker will add extra rows
            // to the other key pickers.
            if (loaded)
            {
                leftMultiPicker.AddMapping(e.Title, Keys.None, true);
                rightMultiPicker.AddMapping(e.Title, Keys.None, true);
            }
        }

        private void bindingsMultiPicker_KeyMappingRemoved(object sender, int rowNum)
        {
            leftMultiPicker.RemoveMapping(rowNum);
            rightMultiPicker.RemoveMapping(rowNum);
        }

        private void bindingsMultiPicker_KeyMappingsChanged(object sender, EventArgs e)
        {
            leftMultiPicker.KeyMappingTitles = bindingsMultiPicker.KeyMappingTitles;
            rightMultiPicker.KeyMappingTitles = bindingsMultiPicker.KeyMappingTitles;
        }
    }
}
