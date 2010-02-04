/*************************************************************************
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * Copyright (C) 2009, 2010 Steven Robertson <steve@playnode.org>
 *
 * Windar: Playdar for Windows
 *
 * Windar is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Windar is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 ************************************************************************/

namespace Windar.TrayApp
{
    partial class WaitingDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        System.ComponentModel.IContainer components = null;

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
        void InitializeComponent()
        {
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.borderPanel = new System.Windows.Forms.Panel();
            this.outerPanel = new System.Windows.Forms.Panel();
            this.borderPanel.SuspendLayout();
            this.outerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(10, 10);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(300, 23);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 0;
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(7, 36);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(56, 13);
            this.StatusLabel.TabIndex = 1;
            this.StatusLabel.Text = "Working...";
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // borderPanel
            // 
            this.borderPanel.BackColor = System.Drawing.SystemColors.Control;
            this.borderPanel.Controls.Add(this.progressBar);
            this.borderPanel.Controls.Add(this.StatusLabel);
            this.borderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.borderPanel.Location = new System.Drawing.Point(1, 1);
            this.borderPanel.Name = "borderPanel";
            this.borderPanel.Size = new System.Drawing.Size(322, 57);
            this.borderPanel.TabIndex = 2;
            // 
            // outerPanel
            // 
            this.outerPanel.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.outerPanel.Controls.Add(this.borderPanel);
            this.outerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outerPanel.Location = new System.Drawing.Point(0, 0);
            this.outerPanel.Name = "outerPanel";
            this.outerPanel.Padding = new System.Windows.Forms.Padding(1);
            this.outerPanel.Size = new System.Drawing.Size(324, 59);
            this.outerPanel.TabIndex = 3;
            // 
            // WaitingDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 59);
            this.ControlBox = false;
            this.Controls.Add(this.outerPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(324, 59);
            this.MinimumSize = new System.Drawing.Size(324, 59);
            this.Name = "WaitingDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Please wait";
            this.Shown += new System.EventHandler(this.WaitingDialog_Shown);
            this.borderPanel.ResumeLayout(false);
            this.borderPanel.PerformLayout();
            this.outerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        System.Windows.Forms.ProgressBar progressBar;
        System.ComponentModel.BackgroundWorker backgroundWorker;
        internal System.Windows.Forms.Label StatusLabel;
        System.Windows.Forms.Panel borderPanel;
        System.Windows.Forms.Panel outerPanel;
    }
}