﻿namespace TTMulti.Forms
{
    partial class WindowGroupsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rightToonLbl = new System.Windows.Forms.Label();
            this.leftToonLbl = new System.Windows.Forms.Label();
            this.rightToonCrosshair = new TTMulti.Controls.SelectWindowCrosshair();
            this.leftToonCrosshair = new TTMulti.Controls.SelectWindowCrosshair();
            this.addGroupBtn = new System.Windows.Forms.Button();
            this.removeGroupBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(190, 87);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.rightToonLbl);
            this.groupBox1.Controls.Add(this.leftToonLbl);
            this.groupBox1.Controls.Add(this.rightToonCrosshair);
            this.groupBox1.Controls.Add(this.leftToonCrosshair);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(184, 81);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Group 1";
            // 
            // rightToonLbl
            // 
            this.rightToonLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rightToonLbl.AutoSize = true;
            this.rightToonLbl.Location = new System.Drawing.Point(112, 18);
            this.rightToonLbl.Name = "rightToonLbl";
            this.rightToonLbl.Size = new System.Drawing.Size(60, 13);
            this.rightToonLbl.TabIndex = 3;
            this.rightToonLbl.Text = "Right Toon";
            // 
            // leftToonLbl
            // 
            this.leftToonLbl.AutoSize = true;
            this.leftToonLbl.Location = new System.Drawing.Point(10, 18);
            this.leftToonLbl.Name = "leftToonLbl";
            this.leftToonLbl.Size = new System.Drawing.Size(53, 13);
            this.leftToonLbl.TabIndex = 2;
            this.leftToonLbl.Text = "Left Toon";
            // 
            // rightToonCrosshair
            // 
            this.rightToonCrosshair.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rightToonCrosshair.BackColor = System.Drawing.SystemColors.Control;
            this.rightToonCrosshair.Location = new System.Drawing.Point(124, 34);
            this.rightToonCrosshair.Name = "rightToonCrosshair";
// TODO: Code generation for '' failed because of Exception 'Invalid Primitive Type: System.IntPtr. Consider using CodeObjectCreateExpression.'.
            this.rightToonCrosshair.Size = new System.Drawing.Size(38, 35);
            this.rightToonCrosshair.TabIndex = 1;
            this.rightToonCrosshair.Tag = "right";
            this.rightToonCrosshair.WindowSelected += new TTMulti.Controls.WindowSelectedHandler(this.crosshair_WindowSelected);
            // 
            // leftToonCrosshair
            // 
            this.leftToonCrosshair.BackColor = System.Drawing.SystemColors.Control;
            this.leftToonCrosshair.Location = new System.Drawing.Point(19, 34);
            this.leftToonCrosshair.Name = "leftToonCrosshair";
// TODO: Code generation for '' failed because of Exception 'Invalid Primitive Type: System.IntPtr. Consider using CodeObjectCreateExpression.'.
            this.leftToonCrosshair.Size = new System.Drawing.Size(38, 35);
            this.leftToonCrosshair.TabIndex = 0;
            this.leftToonCrosshair.Tag = "left";
            this.leftToonCrosshair.WindowSelected += new TTMulti.Controls.WindowSelectedHandler(this.crosshair_WindowSelected);
            // 
            // addGroupBtn
            // 
            this.addGroupBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addGroupBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addGroupBtn.Location = new System.Drawing.Point(200, 12);
            this.addGroupBtn.Name = "addGroupBtn";
            this.addGroupBtn.Size = new System.Drawing.Size(34, 31);
            this.addGroupBtn.TabIndex = 1;
            this.addGroupBtn.Text = "+";
            this.addGroupBtn.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.addGroupBtn.UseVisualStyleBackColor = true;
            this.addGroupBtn.Click += new System.EventHandler(this.addGroupBtn_Click);
            // 
            // removeGroupBtn
            // 
            this.removeGroupBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeGroupBtn.Enabled = false;
            this.removeGroupBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.removeGroupBtn.Location = new System.Drawing.Point(200, 50);
            this.removeGroupBtn.Name = "removeGroupBtn";
            this.removeGroupBtn.Size = new System.Drawing.Size(34, 31);
            this.removeGroupBtn.TabIndex = 2;
            this.removeGroupBtn.Text = "-";
            this.removeGroupBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.removeGroupBtn.UseVisualStyleBackColor = true;
            this.removeGroupBtn.Click += new System.EventHandler(this.removeGroupBtn_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(7, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(227, 43);
            this.label1.TabIndex = 3;
            this.label1.Text = "Use the number keys to switch between groups in Multi-Mode. Mirror Mode will cont" +
    "rol all groups simultaneously.";
            // 
            // WindowGroupsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(242, 142);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.removeGroupBtn);
            this.Controls.Add(this.addGroupBtn);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WindowGroupsForm";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 0, 50);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Window Groups";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WindowGroupsForm_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private TTMulti.Controls.SelectWindowCrosshair rightToonCrosshair;
        private TTMulti.Controls.SelectWindowCrosshair leftToonCrosshair;
        private System.Windows.Forms.Label rightToonLbl;
        private System.Windows.Forms.Label leftToonLbl;
        private System.Windows.Forms.Button addGroupBtn;
        private System.Windows.Forms.Button removeGroupBtn;
        private System.Windows.Forms.Label label1;
    }
}