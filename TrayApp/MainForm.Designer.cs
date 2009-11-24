/*
 * Windar: Playdar for Windows
 * Copyright (C) 2009 Steven Robertson <steve@playnode.org>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

namespace Windar.TrayApp
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainformBorderPanel = new System.Windows.Forms.Panel();
            this.mainformTabControl = new System.Windows.Forms.TabControl();
            this.aboutTabPage = new System.Windows.Forms.TabPage();
            this.versionLabel = new System.Windows.Forms.Label();
            this.aboutCenterPanel = new System.Windows.Forms.Panel();
            this.playdarLink = new System.Windows.Forms.LinkLabel();
            this.playdarInfoBox = new System.Windows.Forms.GroupBox();
            this.playdarInfo = new System.Windows.Forms.RichTextBox();
            this.playdarLogo = new System.Windows.Forms.PictureBox();
            this.libraryTabPage = new System.Windows.Forms.TabPage();
            this.networkTabPage = new System.Windows.Forms.TabPage();
            this.playdarTabPage = new System.Windows.Forms.TabPage();
            this.playdarBorderPanel = new System.Windows.Forms.Panel();
            this.playdarBrowser = new System.Windows.Forms.WebBrowser();
            this.logTabPage = new System.Windows.Forms.TabPage();
            this.logBox = new System.Windows.Forms.RichTextBox();
            this.mainformBorderPanel.SuspendLayout();
            this.mainformTabControl.SuspendLayout();
            this.aboutTabPage.SuspendLayout();
            this.aboutCenterPanel.SuspendLayout();
            this.playdarInfoBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.playdarLogo)).BeginInit();
            this.playdarTabPage.SuspendLayout();
            this.playdarBorderPanel.SuspendLayout();
            this.logTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainformBorderPanel
            // 
            this.mainformBorderPanel.Controls.Add(this.mainformTabControl);
            this.mainformBorderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainformBorderPanel.Location = new System.Drawing.Point(0, 0);
            this.mainformBorderPanel.Name = "mainformBorderPanel";
            this.mainformBorderPanel.Padding = new System.Windows.Forms.Padding(4, 6, 4, 4);
            this.mainformBorderPanel.Size = new System.Drawing.Size(624, 442);
            this.mainformBorderPanel.TabIndex = 6;
            // 
            // mainformTabControl
            // 
            this.mainformTabControl.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.mainformTabControl.Controls.Add(this.aboutTabPage);
            this.mainformTabControl.Controls.Add(this.libraryTabPage);
            this.mainformTabControl.Controls.Add(this.networkTabPage);
            this.mainformTabControl.Controls.Add(this.playdarTabPage);
            this.mainformTabControl.Controls.Add(this.logTabPage);
            this.mainformTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainformTabControl.HotTrack = true;
            this.mainformTabControl.Location = new System.Drawing.Point(4, 6);
            this.mainformTabControl.Margin = new System.Windows.Forms.Padding(0);
            this.mainformTabControl.Name = "mainformTabControl";
            this.mainformTabControl.SelectedIndex = 0;
            this.mainformTabControl.Size = new System.Drawing.Size(616, 432);
            this.mainformTabControl.TabIndex = 1;
            this.mainformTabControl.SelectedIndexChanged += new System.EventHandler(this.mainformTabControl_SelectedIndexChanged);
            // 
            // aboutTabPage
            // 
            this.aboutTabPage.Controls.Add(this.versionLabel);
            this.aboutTabPage.Controls.Add(this.aboutCenterPanel);
            this.aboutTabPage.Location = new System.Drawing.Point(4, 25);
            this.aboutTabPage.Margin = new System.Windows.Forms.Padding(0);
            this.aboutTabPage.Name = "aboutTabPage";
            this.aboutTabPage.Size = new System.Drawing.Size(608, 403);
            this.aboutTabPage.TabIndex = 0;
            this.aboutTabPage.Text = "About";
            this.aboutTabPage.UseVisualStyleBackColor = true;
            // 
            // versionLabel
            // 
            this.versionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.versionLabel.AutoSize = true;
            this.versionLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.versionLabel.Location = new System.Drawing.Point(-1, 389);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(82, 13);
            this.versionLabel.TabIndex = 4;
            this.versionLabel.Text = "Windar: Version";
            // 
            // aboutCenterPanel
            // 
            this.aboutCenterPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.aboutCenterPanel.Controls.Add(this.playdarLink);
            this.aboutCenterPanel.Controls.Add(this.playdarInfoBox);
            this.aboutCenterPanel.Controls.Add(this.playdarLogo);
            this.aboutCenterPanel.Location = new System.Drawing.Point(16, 16);
            this.aboutCenterPanel.Name = "aboutCenterPanel";
            this.aboutCenterPanel.Size = new System.Drawing.Size(577, 274);
            this.aboutCenterPanel.TabIndex = 4;
            // 
            // playdarLink
            // 
            this.playdarLink.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.playdarLink.AutoSize = true;
            this.playdarLink.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playdarLink.Location = new System.Drawing.Point(266, 181);
            this.playdarLink.Name = "playdarLink";
            this.playdarLink.Size = new System.Drawing.Size(117, 16);
            this.playdarLink.TabIndex = 3;
            this.playdarLink.TabStop = true;
            this.playdarLink.Text = "www.playdar.org";
            this.playdarLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.playdarLink_LinkClicked);
            // 
            // playdarInfoBox
            // 
            this.playdarInfoBox.Controls.Add(this.playdarInfo);
            this.playdarInfoBox.Location = new System.Drawing.Point(262, 69);
            this.playdarInfoBox.Name = "playdarInfoBox";
            this.playdarInfoBox.Padding = new System.Windows.Forms.Padding(7, 0, 7, 7);
            this.playdarInfoBox.Size = new System.Drawing.Size(307, 109);
            this.playdarInfoBox.TabIndex = 2;
            this.playdarInfoBox.TabStop = false;
            // 
            // playdarInfo
            // 
            this.playdarInfo.BackColor = System.Drawing.SystemColors.Control;
            this.playdarInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.playdarInfo.Cursor = System.Windows.Forms.Cursors.Default;
            this.playdarInfo.DetectUrls = false;
            this.playdarInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playdarInfo.Enabled = false;
            this.playdarInfo.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playdarInfo.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.playdarInfo.Location = new System.Drawing.Point(7, 13);
            this.playdarInfo.Name = "playdarInfo";
            this.playdarInfo.ReadOnly = true;
            this.playdarInfo.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.playdarInfo.ShortcutsEnabled = false;
            this.playdarInfo.Size = new System.Drawing.Size(293, 89);
            this.playdarInfo.TabIndex = 0;
            this.playdarInfo.Text = "Playdar is designed to solve one problem: given the name of a track, find me a wa" +
                "y to listen to it right now. Playdar searches this computer, and potentially fur" +
                "ther afield, until it finds that track.";
            // 
            // playdarLogo
            // 
            this.playdarLogo.Image = ((System.Drawing.Image)(resources.GetObject("playdarLogo.Image")));
            this.playdarLogo.Location = new System.Drawing.Point(3, 9);
            this.playdarLogo.Name = "playdarLogo";
            this.playdarLogo.Size = new System.Drawing.Size(257, 256);
            this.playdarLogo.TabIndex = 0;
            this.playdarLogo.TabStop = false;
            // 
            // libraryTabPage
            // 
            this.libraryTabPage.Location = new System.Drawing.Point(4, 25);
            this.libraryTabPage.Name = "libraryTabPage";
            this.libraryTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.libraryTabPage.Size = new System.Drawing.Size(608, 403);
            this.libraryTabPage.TabIndex = 2;
            this.libraryTabPage.Text = "Library";
            this.libraryTabPage.UseVisualStyleBackColor = true;
            // 
            // networkTabPage
            // 
            this.networkTabPage.Location = new System.Drawing.Point(4, 25);
            this.networkTabPage.Name = "networkTabPage";
            this.networkTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.networkTabPage.Size = new System.Drawing.Size(608, 403);
            this.networkTabPage.TabIndex = 3;
            this.networkTabPage.Text = "Network";
            this.networkTabPage.UseVisualStyleBackColor = true;
            // 
            // playdarTabPage
            // 
            this.playdarTabPage.Controls.Add(this.playdarBorderPanel);
            this.playdarTabPage.Location = new System.Drawing.Point(4, 25);
            this.playdarTabPage.Name = "playdarTabPage";
            this.playdarTabPage.Size = new System.Drawing.Size(608, 403);
            this.playdarTabPage.TabIndex = 1;
            this.playdarTabPage.Text = "Daemon";
            this.playdarTabPage.UseVisualStyleBackColor = true;
            // 
            // playdarBorderPanel
            // 
            this.playdarBorderPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.playdarBorderPanel.Controls.Add(this.playdarBrowser);
            this.playdarBorderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playdarBorderPanel.Location = new System.Drawing.Point(0, 0);
            this.playdarBorderPanel.Name = "playdarBorderPanel";
            this.playdarBorderPanel.Size = new System.Drawing.Size(608, 403);
            this.playdarBorderPanel.TabIndex = 1;
            // 
            // playdarBrowser
            // 
            this.playdarBrowser.AllowWebBrowserDrop = false;
            this.playdarBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playdarBrowser.IsWebBrowserContextMenuEnabled = false;
            this.playdarBrowser.Location = new System.Drawing.Point(0, 0);
            this.playdarBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.playdarBrowser.Name = "playdarBrowser";
            this.playdarBrowser.ScriptErrorsSuppressed = true;
            this.playdarBrowser.Size = new System.Drawing.Size(606, 401);
            this.playdarBrowser.TabIndex = 0;
            this.playdarBrowser.WebBrowserShortcutsEnabled = false;
            this.playdarBrowser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.playdarBrowser_Navigating);
            this.playdarBrowser.NewWindow += new System.ComponentModel.CancelEventHandler(this.playdarBrowser_NewWindow);
            this.playdarBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.playdarBrowser_DocumentCompleted);
            // 
            // logTabPage
            // 
            this.logTabPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logTabPage.Controls.Add(this.logBox);
            this.logTabPage.Location = new System.Drawing.Point(4, 25);
            this.logTabPage.Name = "logTabPage";
            this.logTabPage.Size = new System.Drawing.Size(608, 403);
            this.logTabPage.TabIndex = 4;
            this.logTabPage.Text = "Log";
            this.logTabPage.UseVisualStyleBackColor = true;
            // 
            // logBox
            // 
            this.logBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.logBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logBox.Location = new System.Drawing.Point(0, 0);
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.Size = new System.Drawing.Size(606, 401);
            this.logBox.TabIndex = 0;
            this.logBox.Text = "";
            this.logBox.WordWrap = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 442);
            this.Controls.Add(this.mainformBorderPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Windar - Playdar for Windows";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResizeBegin += new System.EventHandler(this.MainForm_ResizeBegin);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            this.mainformBorderPanel.ResumeLayout(false);
            this.mainformTabControl.ResumeLayout(false);
            this.aboutTabPage.ResumeLayout(false);
            this.aboutTabPage.PerformLayout();
            this.aboutCenterPanel.ResumeLayout(false);
            this.aboutCenterPanel.PerformLayout();
            this.playdarInfoBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.playdarLogo)).EndInit();
            this.playdarTabPage.ResumeLayout(false);
            this.playdarBorderPanel.ResumeLayout(false);
            this.logTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel mainformBorderPanel;
        private System.Windows.Forms.TabControl mainformTabControl;
        private System.Windows.Forms.TabPage aboutTabPage;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Panel aboutCenterPanel;
        private System.Windows.Forms.LinkLabel playdarLink;
        private System.Windows.Forms.GroupBox playdarInfoBox;
        private System.Windows.Forms.RichTextBox playdarInfo;
        private System.Windows.Forms.PictureBox playdarLogo;
        private System.Windows.Forms.TabPage libraryTabPage;
        private System.Windows.Forms.TabPage networkTabPage;
        private System.Windows.Forms.TabPage playdarTabPage;
        private System.Windows.Forms.Panel playdarBorderPanel;
        private System.Windows.Forms.WebBrowser playdarBrowser;
        private System.Windows.Forms.TabPage logTabPage;
        private System.Windows.Forms.RichTextBox logBox;
    }
}