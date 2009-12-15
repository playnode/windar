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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainformBorderPanel = new System.Windows.Forms.Panel();
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.aboutTabPage = new System.Windows.Forms.TabPage();
            this.versionLabel = new System.Windows.Forms.Label();
            this.aboutCenterPanel = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.playdarLink = new System.Windows.Forms.LinkLabel();
            this.playdarLogo = new System.Windows.Forms.PictureBox();
            this.optionsTabPage = new System.Windows.Forms.TabPage();
            this.optionsTabControl = new System.Windows.Forms.TabControl();
            this.generalOptionsTabPage = new System.Windows.Forms.TabPage();
            this.generalOptionsPanel = new System.Windows.Forms.Panel();
            this.generalOptionsCancelButton = new System.Windows.Forms.Button();
            this.generalOptionsSaveButton = new System.Windows.Forms.Button();
            this.aboutGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.nodeNameTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.portTextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.allowIncomingCheckBox = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.autostartCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.forwardCheckBox = new System.Windows.Forms.CheckBox();
            this.peersGroupBox = new System.Windows.Forms.GroupBox();
            this.peersGrid = new System.Windows.Forms.DataGridView();
            this.peerAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.peerPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.peerShare = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.peersContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addPeerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removePeerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.libraryTabPage = new System.Windows.Forms.TabPage();
            this.libraryPanel = new System.Windows.Forms.Panel();
            this.libraryGrid = new System.Windows.Forms.DataGridView();
            this.LibraryItemPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.libraryContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addLibPathMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeLibPathMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.libraryCancelButton = new System.Windows.Forms.Button();
            this.librarySaveButton = new System.Windows.Forms.Button();
            this.modsTabPage = new System.Windows.Forms.TabPage();
            this.modsPanel = new System.Windows.Forms.Panel();
            this.modsGrid = new System.Windows.Forms.DataGridView();
            this.ModuleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModuleWeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModuleTargetTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModuleLocalOnly = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ModuleEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.modsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addModMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeModMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modsCancelButton = new System.Windows.Forms.Button();
            this.modsSaveButton = new System.Windows.Forms.Button();
            this.pluginsTabPage = new System.Windows.Forms.TabPage();
            this.pluginsPanel = new System.Windows.Forms.Panel();
            this.pluginsGrid = new System.Windows.Forms.DataGridView();
            this.PluginName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PluginEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.pluginsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addPluginMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removePluginMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pluginsCancelButton = new System.Windows.Forms.Button();
            this.pluginsSaveButton = new System.Windows.Forms.Button();
            this.propsTabPage = new System.Windows.Forms.TabPage();
            this.propsPanel = new System.Windows.Forms.Panel();
            this.propsCancelButton = new System.Windows.Forms.Button();
            this.propsSaveButton = new System.Windows.Forms.Button();
            this.propsGrid = new System.Windows.Forms.DataGridView();
            this.PropertyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PropertyValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.propsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addPropMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removePropMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playdarTabPage = new System.Windows.Forms.TabPage();
            this.daemonSplitPanel = new System.Windows.Forms.SplitContainer();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.RestartDaemonButton = new System.Windows.Forms.Button();
            this.BackButton = new System.Windows.Forms.Button();
            this.HomeButton = new System.Windows.Forms.Button();
            this.StopDaemonButton = new System.Windows.Forms.Button();
            this.StartDaemonButton = new System.Windows.Forms.Button();
            this.playdarBorderPanel = new System.Windows.Forms.Panel();
            this.PlaydarBrowser = new System.Windows.Forms.WebBrowser();
            this.logTabPage = new System.Windows.Forms.TabPage();
            this.logSplitPanel = new System.Windows.Forms.SplitContainer();
            this.followTailCheckBox = new System.Windows.Forms.CheckBox();
            this.logBoxPanel = new System.Windows.Forms.Panel();
            this.logBox = new Windar.TrayApp.LogTextBox();
            this.mainformBorderPanel.SuspendLayout();
            this.MainTabControl.SuspendLayout();
            this.aboutTabPage.SuspendLayout();
            this.aboutCenterPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.playdarLogo)).BeginInit();
            this.optionsTabPage.SuspendLayout();
            this.optionsTabControl.SuspendLayout();
            this.generalOptionsTabPage.SuspendLayout();
            this.generalOptionsPanel.SuspendLayout();
            this.aboutGroupBox.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.peersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.peersGrid)).BeginInit();
            this.peersContextMenu.SuspendLayout();
            this.libraryTabPage.SuspendLayout();
            this.libraryPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.libraryGrid)).BeginInit();
            this.libraryContextMenu.SuspendLayout();
            this.modsTabPage.SuspendLayout();
            this.modsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.modsGrid)).BeginInit();
            this.modsContextMenu.SuspendLayout();
            this.pluginsTabPage.SuspendLayout();
            this.pluginsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pluginsGrid)).BeginInit();
            this.pluginsContextMenu.SuspendLayout();
            this.propsTabPage.SuspendLayout();
            this.propsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.propsGrid)).BeginInit();
            this.propsContextMenu.SuspendLayout();
            this.playdarTabPage.SuspendLayout();
            this.daemonSplitPanel.Panel1.SuspendLayout();
            this.daemonSplitPanel.Panel2.SuspendLayout();
            this.daemonSplitPanel.SuspendLayout();
            this.playdarBorderPanel.SuspendLayout();
            this.logTabPage.SuspendLayout();
            this.logSplitPanel.Panel1.SuspendLayout();
            this.logSplitPanel.Panel2.SuspendLayout();
            this.logSplitPanel.SuspendLayout();
            this.logBoxPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainformBorderPanel
            // 
            this.mainformBorderPanel.Controls.Add(this.MainTabControl);
            this.mainformBorderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainformBorderPanel.Location = new System.Drawing.Point(0, 0);
            this.mainformBorderPanel.Name = "mainformBorderPanel";
            this.mainformBorderPanel.Padding = new System.Windows.Forms.Padding(4, 6, 4, 4);
            this.mainformBorderPanel.Size = new System.Drawing.Size(624, 442);
            this.mainformBorderPanel.TabIndex = 6;
            // 
            // MainTabControl
            // 
            this.MainTabControl.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.MainTabControl.Controls.Add(this.aboutTabPage);
            this.MainTabControl.Controls.Add(this.optionsTabPage);
            this.MainTabControl.Controls.Add(this.playdarTabPage);
            this.MainTabControl.Controls.Add(this.logTabPage);
            this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTabControl.HotTrack = true;
            this.MainTabControl.Location = new System.Drawing.Point(4, 6);
            this.MainTabControl.Margin = new System.Windows.Forms.Padding(0);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(616, 432);
            this.MainTabControl.TabIndex = 1;
            this.MainTabControl.Deselecting += new System.Windows.Forms.TabControlCancelEventHandler(this.MainTabControl_Deselecting);
            this.MainTabControl.SelectedIndexChanged += new System.EventHandler(this.MainTabControl_SelectedIndexChanged);
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
            this.versionLabel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
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
            this.aboutCenterPanel.Controls.Add(this.label7);
            this.aboutCenterPanel.Controls.Add(this.pictureBox1);
            this.aboutCenterPanel.Controls.Add(this.playdarLink);
            this.aboutCenterPanel.Controls.Add(this.playdarLogo);
            this.aboutCenterPanel.Location = new System.Drawing.Point(13, 50);
            this.aboutCenterPanel.Name = "aboutCenterPanel";
            this.aboutCenterPanel.Size = new System.Drawing.Size(577, 274);
            this.aboutCenterPanel.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label7.Location = new System.Drawing.Point(294, 136);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(177, 20);
            this.label7.TabIndex = 5;
            this.label7.Text = "Music Content Resolver";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Windar.TrayApp.Properties.Resources.playdar_text;
            this.pictureBox1.Location = new System.Drawing.Point(296, 88);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(228, 45);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // playdarLink
            // 
            this.playdarLink.ActiveLinkColor = System.Drawing.Color.FromArgb(((int) (((byte) (128)))), ((int) (((byte) (128)))), ((int) (((byte) (255)))));
            this.playdarLink.AutoSize = true;
            this.playdarLink.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.playdarLink.Location = new System.Drawing.Point(295, 159);
            this.playdarLink.Name = "playdarLink";
            this.playdarLink.Size = new System.Drawing.Size(117, 16);
            this.playdarLink.TabIndex = 3;
            this.playdarLink.TabStop = true;
            this.playdarLink.Text = "www.playdar.org";
            this.playdarLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.playdarLink_LinkClicked);
            // 
            // playdarLogo
            // 
            this.playdarLogo.Image = ((System.Drawing.Image) (resources.GetObject("playdarLogo.Image")));
            this.playdarLogo.Location = new System.Drawing.Point(3, 9);
            this.playdarLogo.Name = "playdarLogo";
            this.playdarLogo.Size = new System.Drawing.Size(257, 256);
            this.playdarLogo.TabIndex = 0;
            this.playdarLogo.TabStop = false;
            // 
            // optionsTabPage
            // 
            this.optionsTabPage.Controls.Add(this.optionsTabControl);
            this.optionsTabPage.Location = new System.Drawing.Point(4, 25);
            this.optionsTabPage.Margin = new System.Windows.Forms.Padding(0);
            this.optionsTabPage.Name = "optionsTabPage";
            this.optionsTabPage.Size = new System.Drawing.Size(608, 403);
            this.optionsTabPage.TabIndex = 3;
            this.optionsTabPage.Text = "Settings";
            this.optionsTabPage.UseVisualStyleBackColor = true;
            // 
            // optionsTabControl
            // 
            this.optionsTabControl.Controls.Add(this.generalOptionsTabPage);
            this.optionsTabControl.Controls.Add(this.libraryTabPage);
            this.optionsTabControl.Controls.Add(this.modsTabPage);
            this.optionsTabControl.Controls.Add(this.pluginsTabPage);
            this.optionsTabControl.Controls.Add(this.propsTabPage);
            this.optionsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.optionsTabControl.Location = new System.Drawing.Point(0, 0);
            this.optionsTabControl.Name = "optionsTabControl";
            this.optionsTabControl.Padding = new System.Drawing.Point(0, 0);
            this.optionsTabControl.SelectedIndex = 0;
            this.optionsTabControl.Size = new System.Drawing.Size(608, 403);
            this.optionsTabControl.TabIndex = 17;
            this.optionsTabControl.Deselecting += new System.Windows.Forms.TabControlCancelEventHandler(this.optionsTabControl_Deselecting);
            this.optionsTabControl.SelectedIndexChanged += new System.EventHandler(this.optionsTabControl_SelectedIndexChanged);
            // 
            // generalOptionsTabPage
            // 
            this.generalOptionsTabPage.Controls.Add(this.generalOptionsPanel);
            this.generalOptionsTabPage.Location = new System.Drawing.Point(4, 22);
            this.generalOptionsTabPage.Name = "generalOptionsTabPage";
            this.generalOptionsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.generalOptionsTabPage.Size = new System.Drawing.Size(600, 377);
            this.generalOptionsTabPage.TabIndex = 2;
            this.generalOptionsTabPage.Text = "General Options";
            this.generalOptionsTabPage.UseVisualStyleBackColor = true;
            // 
            // generalOptionsPanel
            // 
            this.generalOptionsPanel.BackColor = System.Drawing.SystemColors.Control;
            this.generalOptionsPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.generalOptionsPanel.Controls.Add(this.generalOptionsCancelButton);
            this.generalOptionsPanel.Controls.Add(this.generalOptionsSaveButton);
            this.generalOptionsPanel.Controls.Add(this.aboutGroupBox);
            this.generalOptionsPanel.Controls.Add(this.peersGroupBox);
            this.generalOptionsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.generalOptionsPanel.Location = new System.Drawing.Point(3, 3);
            this.generalOptionsPanel.Name = "generalOptionsPanel";
            this.generalOptionsPanel.Padding = new System.Windows.Forms.Padding(6);
            this.generalOptionsPanel.Size = new System.Drawing.Size(594, 371);
            this.generalOptionsPanel.TabIndex = 0;
            // 
            // generalOptionsCancelButton
            // 
            this.generalOptionsCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.generalOptionsCancelButton.Enabled = false;
            this.generalOptionsCancelButton.Location = new System.Drawing.Point(75, 337);
            this.generalOptionsCancelButton.Name = "generalOptionsCancelButton";
            this.generalOptionsCancelButton.Size = new System.Drawing.Size(60, 23);
            this.generalOptionsCancelButton.TabIndex = 17;
            this.generalOptionsCancelButton.Text = "Cancel";
            this.generalOptionsCancelButton.UseVisualStyleBackColor = true;
            this.generalOptionsCancelButton.Click += new System.EventHandler(this.generalOptionsCancelButton_Click);
            // 
            // generalOptionsSaveButton
            // 
            this.generalOptionsSaveButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.generalOptionsSaveButton.Enabled = false;
            this.generalOptionsSaveButton.Location = new System.Drawing.Point(9, 337);
            this.generalOptionsSaveButton.Name = "generalOptionsSaveButton";
            this.generalOptionsSaveButton.Size = new System.Drawing.Size(60, 23);
            this.generalOptionsSaveButton.TabIndex = 16;
            this.generalOptionsSaveButton.Text = "Save";
            this.generalOptionsSaveButton.UseVisualStyleBackColor = true;
            this.generalOptionsSaveButton.Click += new System.EventHandler(this.generalOptionsSaveButton_Click);
            // 
            // aboutGroupBox
            // 
            this.aboutGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.aboutGroupBox.Controls.Add(this.tableLayoutPanel1);
            this.aboutGroupBox.Controls.Add(this.tableLayoutPanel2);
            this.aboutGroupBox.Location = new System.Drawing.Point(9, 9);
            this.aboutGroupBox.Name = "aboutGroupBox";
            this.aboutGroupBox.Size = new System.Drawing.Size(572, 117);
            this.aboutGroupBox.TabIndex = 14;
            this.aboutGroupBox.TabStop = false;
            this.aboutGroupBox.Text = "This Computer";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 96F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 129F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.nodeNameTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.portTextBox, 3, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 19);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(325, 24);
            this.tableLayoutPanel1.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Node name";
            // 
            // nodeNameTextBox
            // 
            this.nodeNameTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.nodeNameTextBox.Location = new System.Drawing.Point(99, 3);
            this.nodeNameTextBox.Name = "nodeNameTextBox";
            this.nodeNameTextBox.Size = new System.Drawing.Size(120, 20);
            this.nodeNameTextBox.TabIndex = 0;
            this.nodeNameTextBox.TextChanged += new System.EventHandler(this.nodeNameTextBox_TextChanged);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(236, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Port";
            // 
            // portTextBox
            // 
            this.portTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.portTextBox.Location = new System.Drawing.Point(268, 3);
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.Size = new System.Drawing.Size(50, 20);
            this.portTextBox.TabIndex = 5;
            this.portTextBox.TextChanged += new System.EventHandler(this.portTextBox_TextChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 88.64629F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.35371F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 196F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            this.tableLayoutPanel2.Controls.Add(this.allowIncomingCheckBox, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label6, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.autostartCheckBox, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.forwardCheckBox, 1, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(6, 49);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(479, 62);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // allowIncomingCheckBox
            // 
            this.allowIncomingCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.allowIncomingCheckBox.AutoSize = true;
            this.allowIncomingCheckBox.Location = new System.Drawing.Point(184, 3);
            this.allowIncomingCheckBox.Name = "allowIncomingCheckBox";
            this.allowIncomingCheckBox.Size = new System.Drawing.Size(15, 14);
            this.allowIncomingCheckBox.TabIndex = 2;
            this.allowIncomingCheckBox.UseVisualStyleBackColor = true;
            this.allowIncomingCheckBox.CheckedChanged += new System.EventHandler(this.allowIncomingCheckBox_CheckedChanged);
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(225, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(172, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Automatically start Windar on login:";
            // 
            // autostartCheckBox
            // 
            this.autostartCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.autostartCheckBox.AutoSize = true;
            this.autostartCheckBox.Location = new System.Drawing.Point(403, 3);
            this.autostartCheckBox.Name = "autostartCheckBox";
            this.autostartCheckBox.Size = new System.Drawing.Size(15, 14);
            this.autostartCheckBox.TabIndex = 11;
            this.autostartCheckBox.UseVisualStyleBackColor = true;
            this.autostartCheckBox.CheckedChanged += new System.EventHandler(this.autostartCheckBox_CheckedChanged);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Allow incoming connections:";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(174, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Forward on queries to other nodes :";
            // 
            // forwardCheckBox
            // 
            this.forwardCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.forwardCheckBox.AutoSize = true;
            this.forwardCheckBox.Location = new System.Drawing.Point(184, 23);
            this.forwardCheckBox.Name = "forwardCheckBox";
            this.forwardCheckBox.Size = new System.Drawing.Size(15, 14);
            this.forwardCheckBox.TabIndex = 9;
            this.forwardCheckBox.UseVisualStyleBackColor = true;
            this.forwardCheckBox.CheckedChanged += new System.EventHandler(this.forwardCheckBox_CheckedChanged);
            // 
            // peersGroupBox
            // 
            this.peersGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.peersGroupBox.Controls.Add(this.peersGrid);
            this.peersGroupBox.Location = new System.Drawing.Point(9, 132);
            this.peersGroupBox.Name = "peersGroupBox";
            this.peersGroupBox.Padding = new System.Windows.Forms.Padding(7, 4, 6, 7);
            this.peersGroupBox.Size = new System.Drawing.Size(572, 199);
            this.peersGroupBox.TabIndex = 15;
            this.peersGroupBox.TabStop = false;
            this.peersGroupBox.Text = "Other Computers";
            // 
            // peersGrid
            // 
            this.peersGrid.AllowUserToAddRows = false;
            this.peersGrid.AllowUserToDeleteRows = false;
            this.peersGrid.AllowUserToResizeRows = false;
            this.peersGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.peersGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.peersGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.peersGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.peersGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.peerAddress,
            this.peerPort,
            this.peerShare});
            this.peersGrid.ContextMenuStrip = this.peersContextMenu;
            this.peersGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.peersGrid.Location = new System.Drawing.Point(7, 17);
            this.peersGrid.Name = "peersGrid";
            this.peersGrid.RowHeadersVisible = false;
            this.peersGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.peersGrid.Size = new System.Drawing.Size(559, 175);
            this.peersGrid.StandardTab = true;
            this.peersGrid.TabIndex = 9;
            this.peersGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.peersGrid_MouseDown);
            this.peersGrid.MouseClick += new System.Windows.Forms.MouseEventHandler(this.peersGrid_MouseClick);
            // 
            // peerAddress
            // 
            this.peerAddress.FillWeight = 137.5049F;
            this.peerAddress.HeaderText = "DNS name or IP address";
            this.peerAddress.MaxInputLength = 50;
            this.peerAddress.MinimumWidth = 60;
            this.peerAddress.Name = "peerAddress";
            // 
            // peerPort
            // 
            this.peerPort.FillWeight = 66.34409F;
            this.peerPort.HeaderText = "Port";
            this.peerPort.MaxInputLength = 10;
            this.peerPort.MinimumWidth = 30;
            this.peerPort.Name = "peerPort";
            this.peerPort.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // peerShare
            // 
            this.peerShare.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.peerShare.FillWeight = 43.7529F;
            this.peerShare.HeaderText = "Share";
            this.peerShare.MinimumWidth = 50;
            this.peerShare.Name = "peerShare";
            this.peerShare.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.peerShare.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.peerShare.Width = 50;
            // 
            // peersContextMenu
            // 
            this.peersContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPeerMenuItem,
            this.removePeerMenuItem});
            this.peersContextMenu.Name = "peersContextMenu";
            this.peersContextMenu.Size = new System.Drawing.Size(118, 48);
            // 
            // addPeerMenuItem
            // 
            this.addPeerMenuItem.Name = "addPeerMenuItem";
            this.addPeerMenuItem.Size = new System.Drawing.Size(117, 22);
            this.addPeerMenuItem.Text = "Add";
            this.addPeerMenuItem.Click += new System.EventHandler(this.addPeerMenuItem_Click);
            // 
            // removePeerMenuItem
            // 
            this.removePeerMenuItem.Name = "removePeerMenuItem";
            this.removePeerMenuItem.Size = new System.Drawing.Size(117, 22);
            this.removePeerMenuItem.Text = "Remove";
            this.removePeerMenuItem.Click += new System.EventHandler(this.removePeerMenuItem_Click);
            // 
            // libraryTabPage
            // 
            this.libraryTabPage.Controls.Add(this.libraryPanel);
            this.libraryTabPage.Location = new System.Drawing.Point(4, 22);
            this.libraryTabPage.Name = "libraryTabPage";
            this.libraryTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.libraryTabPage.Size = new System.Drawing.Size(600, 377);
            this.libraryTabPage.TabIndex = 7;
            this.libraryTabPage.Text = "Local Library Paths";
            this.libraryTabPage.UseVisualStyleBackColor = true;
            // 
            // libraryPanel
            // 
            this.libraryPanel.BackColor = System.Drawing.SystemColors.Control;
            this.libraryPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.libraryPanel.Controls.Add(this.libraryGrid);
            this.libraryPanel.Controls.Add(this.libraryCancelButton);
            this.libraryPanel.Controls.Add(this.librarySaveButton);
            this.libraryPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.libraryPanel.Location = new System.Drawing.Point(3, 3);
            this.libraryPanel.Name = "libraryPanel";
            this.libraryPanel.Padding = new System.Windows.Forms.Padding(6);
            this.libraryPanel.Size = new System.Drawing.Size(594, 371);
            this.libraryPanel.TabIndex = 1;
            // 
            // libraryGrid
            // 
            this.libraryGrid.AllowUserToAddRows = false;
            this.libraryGrid.AllowUserToDeleteRows = false;
            this.libraryGrid.AllowUserToResizeRows = false;
            this.libraryGrid.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.libraryGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.libraryGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.libraryGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.libraryGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.libraryGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.libraryGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LibraryItemPath});
            this.libraryGrid.ContextMenuStrip = this.libraryContextMenu;
            this.libraryGrid.Location = new System.Drawing.Point(9, 9);
            this.libraryGrid.MultiSelect = false;
            this.libraryGrid.Name = "libraryGrid";
            this.libraryGrid.RowHeadersVisible = false;
            this.libraryGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.libraryGrid.Size = new System.Drawing.Size(572, 318);
            this.libraryGrid.StandardTab = true;
            this.libraryGrid.TabIndex = 22;
            this.libraryGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.libraryGrid_MouseDown);
            // 
            // LibraryItemPath
            // 
            this.LibraryItemPath.HeaderText = "Path";
            this.LibraryItemPath.Name = "LibraryItemPath";
            // 
            // libraryContextMenu
            // 
            this.libraryContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addLibPathMenuItem,
            this.removeLibPathMenuItem});
            this.libraryContextMenu.Name = "libraryContextMenu";
            this.libraryContextMenu.Size = new System.Drawing.Size(118, 48);
            // 
            // addLibPathMenuItem
            // 
            this.addLibPathMenuItem.Name = "addLibPathMenuItem";
            this.addLibPathMenuItem.Size = new System.Drawing.Size(117, 22);
            this.addLibPathMenuItem.Text = "Add";
            // 
            // removeLibPathMenuItem
            // 
            this.removeLibPathMenuItem.Name = "removeLibPathMenuItem";
            this.removeLibPathMenuItem.Size = new System.Drawing.Size(117, 22);
            this.removeLibPathMenuItem.Text = "Remove";
            // 
            // libraryCancelButton
            // 
            this.libraryCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.libraryCancelButton.Enabled = false;
            this.libraryCancelButton.Location = new System.Drawing.Point(75, 337);
            this.libraryCancelButton.Name = "libraryCancelButton";
            this.libraryCancelButton.Size = new System.Drawing.Size(60, 23);
            this.libraryCancelButton.TabIndex = 21;
            this.libraryCancelButton.Text = "Cancel";
            this.libraryCancelButton.UseVisualStyleBackColor = true;
            // 
            // librarySaveButton
            // 
            this.librarySaveButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.librarySaveButton.Enabled = false;
            this.librarySaveButton.Location = new System.Drawing.Point(9, 337);
            this.librarySaveButton.Name = "librarySaveButton";
            this.librarySaveButton.Size = new System.Drawing.Size(60, 23);
            this.librarySaveButton.TabIndex = 20;
            this.librarySaveButton.Text = "Save";
            this.librarySaveButton.UseVisualStyleBackColor = true;
            // 
            // modsTabPage
            // 
            this.modsTabPage.Controls.Add(this.modsPanel);
            this.modsTabPage.Location = new System.Drawing.Point(4, 22);
            this.modsTabPage.Name = "modsTabPage";
            this.modsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.modsTabPage.Size = new System.Drawing.Size(600, 377);
            this.modsTabPage.TabIndex = 4;
            this.modsTabPage.Text = "Resolver Modules";
            this.modsTabPage.UseVisualStyleBackColor = true;
            // 
            // modsPanel
            // 
            this.modsPanel.BackColor = System.Drawing.SystemColors.Control;
            this.modsPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.modsPanel.Controls.Add(this.modsGrid);
            this.modsPanel.Controls.Add(this.modsCancelButton);
            this.modsPanel.Controls.Add(this.modsSaveButton);
            this.modsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modsPanel.Location = new System.Drawing.Point(3, 3);
            this.modsPanel.Name = "modsPanel";
            this.modsPanel.Padding = new System.Windows.Forms.Padding(6);
            this.modsPanel.Size = new System.Drawing.Size(594, 371);
            this.modsPanel.TabIndex = 0;
            // 
            // modsGrid
            // 
            this.modsGrid.AllowUserToAddRows = false;
            this.modsGrid.AllowUserToDeleteRows = false;
            this.modsGrid.AllowUserToResizeRows = false;
            this.modsGrid.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.modsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.modsGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.modsGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.modsGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.modsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.modsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ModuleName,
            this.ModuleWeight,
            this.ModuleTargetTime,
            this.ModuleLocalOnly,
            this.ModuleEnabled});
            this.modsGrid.ContextMenuStrip = this.modsContextMenu;
            this.modsGrid.Location = new System.Drawing.Point(9, 9);
            this.modsGrid.MultiSelect = false;
            this.modsGrid.Name = "modsGrid";
            this.modsGrid.RowHeadersVisible = false;
            this.modsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.modsGrid.Size = new System.Drawing.Size(572, 318);
            this.modsGrid.StandardTab = true;
            this.modsGrid.TabIndex = 22;
            this.modsGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.modsGrid_MouseDown);
            // 
            // ModuleName
            // 
            this.ModuleName.HeaderText = "Name";
            this.ModuleName.Name = "ModuleName";
            // 
            // ModuleWeight
            // 
            this.ModuleWeight.HeaderText = "Weight";
            this.ModuleWeight.Name = "ModuleWeight";
            // 
            // ModuleTargetTime
            // 
            this.ModuleTargetTime.HeaderText = "Target Time";
            this.ModuleTargetTime.Name = "ModuleTargetTime";
            // 
            // ModuleLocalOnly
            // 
            this.ModuleLocalOnly.HeaderText = "Local Only";
            this.ModuleLocalOnly.Name = "ModuleLocalOnly";
            // 
            // ModuleEnabled
            // 
            this.ModuleEnabled.HeaderText = "Enabled";
            this.ModuleEnabled.Name = "ModuleEnabled";
            // 
            // modsContextMenu
            // 
            this.modsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addModMenuItem,
            this.removeModMenuItem});
            this.modsContextMenu.Name = "modsContextMenu";
            this.modsContextMenu.Size = new System.Drawing.Size(118, 48);
            // 
            // addModMenuItem
            // 
            this.addModMenuItem.Name = "addModMenuItem";
            this.addModMenuItem.Size = new System.Drawing.Size(117, 22);
            this.addModMenuItem.Text = "Add";
            // 
            // removeModMenuItem
            // 
            this.removeModMenuItem.Name = "removeModMenuItem";
            this.removeModMenuItem.Size = new System.Drawing.Size(117, 22);
            this.removeModMenuItem.Text = "Remove";
            // 
            // modsCancelButton
            // 
            this.modsCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.modsCancelButton.Enabled = false;
            this.modsCancelButton.Location = new System.Drawing.Point(75, 337);
            this.modsCancelButton.Name = "modsCancelButton";
            this.modsCancelButton.Size = new System.Drawing.Size(60, 23);
            this.modsCancelButton.TabIndex = 21;
            this.modsCancelButton.Text = "Cancel";
            this.modsCancelButton.UseVisualStyleBackColor = true;
            // 
            // modsSaveButton
            // 
            this.modsSaveButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.modsSaveButton.Enabled = false;
            this.modsSaveButton.Location = new System.Drawing.Point(9, 337);
            this.modsSaveButton.Name = "modsSaveButton";
            this.modsSaveButton.Size = new System.Drawing.Size(60, 23);
            this.modsSaveButton.TabIndex = 20;
            this.modsSaveButton.Text = "Save";
            this.modsSaveButton.UseVisualStyleBackColor = true;
            // 
            // pluginsTabPage
            // 
            this.pluginsTabPage.Controls.Add(this.pluginsPanel);
            this.pluginsTabPage.Location = new System.Drawing.Point(4, 22);
            this.pluginsTabPage.Name = "pluginsTabPage";
            this.pluginsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.pluginsTabPage.Size = new System.Drawing.Size(600, 377);
            this.pluginsTabPage.TabIndex = 6;
            this.pluginsTabPage.Text = "Plug-ins";
            this.pluginsTabPage.UseVisualStyleBackColor = true;
            // 
            // pluginsPanel
            // 
            this.pluginsPanel.BackColor = System.Drawing.SystemColors.Control;
            this.pluginsPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pluginsPanel.Controls.Add(this.pluginsGrid);
            this.pluginsPanel.Controls.Add(this.pluginsCancelButton);
            this.pluginsPanel.Controls.Add(this.pluginsSaveButton);
            this.pluginsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pluginsPanel.Location = new System.Drawing.Point(3, 3);
            this.pluginsPanel.Name = "pluginsPanel";
            this.pluginsPanel.Padding = new System.Windows.Forms.Padding(6);
            this.pluginsPanel.Size = new System.Drawing.Size(594, 371);
            this.pluginsPanel.TabIndex = 0;
            // 
            // pluginsGrid
            // 
            this.pluginsGrid.AllowUserToAddRows = false;
            this.pluginsGrid.AllowUserToDeleteRows = false;
            this.pluginsGrid.AllowUserToResizeRows = false;
            this.pluginsGrid.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pluginsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.pluginsGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.pluginsGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pluginsGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.pluginsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.pluginsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PluginName,
            this.PluginEnabled});
            this.pluginsGrid.ContextMenuStrip = this.pluginsContextMenu;
            this.pluginsGrid.Location = new System.Drawing.Point(9, 9);
            this.pluginsGrid.MultiSelect = false;
            this.pluginsGrid.Name = "pluginsGrid";
            this.pluginsGrid.RowHeadersVisible = false;
            this.pluginsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.pluginsGrid.Size = new System.Drawing.Size(572, 318);
            this.pluginsGrid.StandardTab = true;
            this.pluginsGrid.TabIndex = 26;
            this.pluginsGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pluginsGrid_MouseDown);
            // 
            // PluginName
            // 
            this.PluginName.HeaderText = "Name";
            this.PluginName.Name = "PluginName";
            this.PluginName.ReadOnly = true;
            // 
            // PluginEnabled
            // 
            this.PluginEnabled.HeaderText = "Enabled";
            this.PluginEnabled.Name = "PluginEnabled";
            this.PluginEnabled.ReadOnly = true;
            this.PluginEnabled.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.PluginEnabled.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // pluginsContextMenu
            // 
            this.pluginsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPluginMenuItem,
            this.removePluginMenuItem});
            this.pluginsContextMenu.Name = "pluginsContextMenu";
            this.pluginsContextMenu.Size = new System.Drawing.Size(118, 48);
            // 
            // addPluginMenuItem
            // 
            this.addPluginMenuItem.Name = "addPluginMenuItem";
            this.addPluginMenuItem.Size = new System.Drawing.Size(117, 22);
            this.addPluginMenuItem.Text = "Add";
            // 
            // removePluginMenuItem
            // 
            this.removePluginMenuItem.Name = "removePluginMenuItem";
            this.removePluginMenuItem.Size = new System.Drawing.Size(117, 22);
            this.removePluginMenuItem.Text = "Remove";
            // 
            // pluginsCancelButton
            // 
            this.pluginsCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pluginsCancelButton.Enabled = false;
            this.pluginsCancelButton.Location = new System.Drawing.Point(75, 337);
            this.pluginsCancelButton.Name = "pluginsCancelButton";
            this.pluginsCancelButton.Size = new System.Drawing.Size(60, 23);
            this.pluginsCancelButton.TabIndex = 25;
            this.pluginsCancelButton.Text = "Cancel";
            this.pluginsCancelButton.UseVisualStyleBackColor = true;
            // 
            // pluginsSaveButton
            // 
            this.pluginsSaveButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pluginsSaveButton.Enabled = false;
            this.pluginsSaveButton.Location = new System.Drawing.Point(9, 337);
            this.pluginsSaveButton.Name = "pluginsSaveButton";
            this.pluginsSaveButton.Size = new System.Drawing.Size(60, 23);
            this.pluginsSaveButton.TabIndex = 24;
            this.pluginsSaveButton.Text = "Save";
            this.pluginsSaveButton.UseVisualStyleBackColor = true;
            // 
            // propsTabPage
            // 
            this.propsTabPage.Controls.Add(this.propsPanel);
            this.propsTabPage.Location = new System.Drawing.Point(4, 22);
            this.propsTabPage.Name = "propsTabPage";
            this.propsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.propsTabPage.Size = new System.Drawing.Size(600, 377);
            this.propsTabPage.TabIndex = 3;
            this.propsTabPage.Text = "Plug-in Properties";
            this.propsTabPage.UseVisualStyleBackColor = true;
            // 
            // propsPanel
            // 
            this.propsPanel.BackColor = System.Drawing.SystemColors.Control;
            this.propsPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.propsPanel.Controls.Add(this.propsCancelButton);
            this.propsPanel.Controls.Add(this.propsSaveButton);
            this.propsPanel.Controls.Add(this.propsGrid);
            this.propsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propsPanel.Location = new System.Drawing.Point(3, 3);
            this.propsPanel.Name = "propsPanel";
            this.propsPanel.Padding = new System.Windows.Forms.Padding(6);
            this.propsPanel.Size = new System.Drawing.Size(594, 371);
            this.propsPanel.TabIndex = 0;
            // 
            // propsCancelButton
            // 
            this.propsCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.propsCancelButton.Enabled = false;
            this.propsCancelButton.Location = new System.Drawing.Point(75, 337);
            this.propsCancelButton.Name = "propsCancelButton";
            this.propsCancelButton.Size = new System.Drawing.Size(60, 23);
            this.propsCancelButton.TabIndex = 19;
            this.propsCancelButton.Text = "Cancel";
            this.propsCancelButton.UseVisualStyleBackColor = true;
            // 
            // propsSaveButton
            // 
            this.propsSaveButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.propsSaveButton.Enabled = false;
            this.propsSaveButton.Location = new System.Drawing.Point(9, 337);
            this.propsSaveButton.Name = "propsSaveButton";
            this.propsSaveButton.Size = new System.Drawing.Size(60, 23);
            this.propsSaveButton.TabIndex = 18;
            this.propsSaveButton.Text = "Save";
            this.propsSaveButton.UseVisualStyleBackColor = true;
            // 
            // propsGrid
            // 
            this.propsGrid.AllowUserToAddRows = false;
            this.propsGrid.AllowUserToDeleteRows = false;
            this.propsGrid.AllowUserToResizeRows = false;
            this.propsGrid.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.propsGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.propsGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.propsGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.propsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.propsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PropertyName,
            this.PropertyValue});
            this.propsGrid.ContextMenuStrip = this.propsContextMenu;
            this.propsGrid.Location = new System.Drawing.Point(9, 9);
            this.propsGrid.MultiSelect = false;
            this.propsGrid.Name = "propsGrid";
            this.propsGrid.RowHeadersVisible = false;
            this.propsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.propsGrid.Size = new System.Drawing.Size(572, 318);
            this.propsGrid.StandardTab = true;
            this.propsGrid.TabIndex = 1;
            this.propsGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.propsGrid_MouseDown);
            // 
            // PropertyName
            // 
            this.PropertyName.HeaderText = "Name";
            this.PropertyName.Name = "PropertyName";
            this.PropertyName.ReadOnly = true;
            // 
            // PropertyValue
            // 
            this.PropertyValue.HeaderText = "Value";
            this.PropertyValue.Name = "PropertyValue";
            this.PropertyValue.ReadOnly = true;
            // 
            // propsContextMenu
            // 
            this.propsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPropMenuItem,
            this.removePropMenuItem});
            this.propsContextMenu.Name = "propsContextMenu";
            this.propsContextMenu.Size = new System.Drawing.Size(118, 48);
            // 
            // addPropMenuItem
            // 
            this.addPropMenuItem.Name = "addPropMenuItem";
            this.addPropMenuItem.Size = new System.Drawing.Size(117, 22);
            this.addPropMenuItem.Text = "Add";
            // 
            // removePropMenuItem
            // 
            this.removePropMenuItem.Name = "removePropMenuItem";
            this.removePropMenuItem.Size = new System.Drawing.Size(117, 22);
            this.removePropMenuItem.Text = "Remove";
            // 
            // playdarTabPage
            // 
            this.playdarTabPage.Controls.Add(this.daemonSplitPanel);
            this.playdarTabPage.Location = new System.Drawing.Point(4, 25);
            this.playdarTabPage.Name = "playdarTabPage";
            this.playdarTabPage.Size = new System.Drawing.Size(608, 403);
            this.playdarTabPage.TabIndex = 1;
            this.playdarTabPage.Text = "Resolver";
            this.playdarTabPage.UseVisualStyleBackColor = true;
            // 
            // daemonSplitPanel
            // 
            this.daemonSplitPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.daemonSplitPanel.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.daemonSplitPanel.IsSplitterFixed = true;
            this.daemonSplitPanel.Location = new System.Drawing.Point(0, 0);
            this.daemonSplitPanel.Name = "daemonSplitPanel";
            this.daemonSplitPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // daemonSplitPanel.Panel1
            // 
            this.daemonSplitPanel.Panel1.Controls.Add(this.RefreshButton);
            this.daemonSplitPanel.Panel1.Controls.Add(this.RestartDaemonButton);
            this.daemonSplitPanel.Panel1.Controls.Add(this.BackButton);
            this.daemonSplitPanel.Panel1.Controls.Add(this.HomeButton);
            this.daemonSplitPanel.Panel1.Controls.Add(this.StopDaemonButton);
            this.daemonSplitPanel.Panel1.Controls.Add(this.StartDaemonButton);
            // 
            // daemonSplitPanel.Panel2
            // 
            this.daemonSplitPanel.Panel2.Controls.Add(this.playdarBorderPanel);
            this.daemonSplitPanel.Size = new System.Drawing.Size(608, 403);
            this.daemonSplitPanel.SplitterDistance = 25;
            this.daemonSplitPanel.TabIndex = 2;
            // 
            // RefreshButton
            // 
            this.RefreshButton.Location = new System.Drawing.Point(134, 0);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(60, 23);
            this.RefreshButton.TabIndex = 6;
            this.RefreshButton.Text = "Refresh";
            this.RefreshButton.UseVisualStyleBackColor = true;
            this.RefreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // RestartDaemonButton
            // 
            this.RestartDaemonButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RestartDaemonButton.Location = new System.Drawing.Point(546, 0);
            this.RestartDaemonButton.Name = "RestartDaemonButton";
            this.RestartDaemonButton.Size = new System.Drawing.Size(60, 23);
            this.RestartDaemonButton.TabIndex = 5;
            this.RestartDaemonButton.Text = "Restart";
            this.RestartDaemonButton.UseVisualStyleBackColor = true;
            this.RestartDaemonButton.Click += new System.EventHandler(this.restartButton_Click);
            // 
            // BackButton
            // 
            this.BackButton.Enabled = false;
            this.BackButton.Location = new System.Drawing.Point(68, 0);
            this.BackButton.Name = "BackButton";
            this.BackButton.Size = new System.Drawing.Size(60, 23);
            this.BackButton.TabIndex = 4;
            this.BackButton.Text = "Back";
            this.BackButton.UseVisualStyleBackColor = true;
            this.BackButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // HomeButton
            // 
            this.HomeButton.Enabled = false;
            this.HomeButton.Location = new System.Drawing.Point(2, 0);
            this.HomeButton.Name = "HomeButton";
            this.HomeButton.Size = new System.Drawing.Size(60, 23);
            this.HomeButton.TabIndex = 3;
            this.HomeButton.Text = "Home";
            this.HomeButton.UseVisualStyleBackColor = true;
            this.HomeButton.Click += new System.EventHandler(this.homeButton_Click);
            // 
            // StopDaemonButton
            // 
            this.StopDaemonButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.StopDaemonButton.Location = new System.Drawing.Point(480, 0);
            this.StopDaemonButton.Name = "StopDaemonButton";
            this.StopDaemonButton.Size = new System.Drawing.Size(60, 23);
            this.StopDaemonButton.TabIndex = 2;
            this.StopDaemonButton.Text = "Stop";
            this.StopDaemonButton.UseVisualStyleBackColor = true;
            this.StopDaemonButton.Click += new System.EventHandler(this.stopDaemonButton_Click);
            // 
            // StartDaemonButton
            // 
            this.StartDaemonButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.StartDaemonButton.Enabled = false;
            this.StartDaemonButton.Location = new System.Drawing.Point(414, 0);
            this.StartDaemonButton.Name = "StartDaemonButton";
            this.StartDaemonButton.Size = new System.Drawing.Size(60, 23);
            this.StartDaemonButton.TabIndex = 1;
            this.StartDaemonButton.Text = "Start";
            this.StartDaemonButton.UseVisualStyleBackColor = true;
            this.StartDaemonButton.Click += new System.EventHandler(this.startDaemonButton_Click);
            // 
            // playdarBorderPanel
            // 
            this.playdarBorderPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.playdarBorderPanel.Controls.Add(this.PlaydarBrowser);
            this.playdarBorderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playdarBorderPanel.Location = new System.Drawing.Point(0, 0);
            this.playdarBorderPanel.Name = "playdarBorderPanel";
            this.playdarBorderPanel.Size = new System.Drawing.Size(608, 374);
            this.playdarBorderPanel.TabIndex = 1;
            // 
            // PlaydarBrowser
            // 
            this.PlaydarBrowser.AllowWebBrowserDrop = false;
            this.PlaydarBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PlaydarBrowser.IsWebBrowserContextMenuEnabled = false;
            this.PlaydarBrowser.Location = new System.Drawing.Point(0, 0);
            this.PlaydarBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.PlaydarBrowser.Name = "PlaydarBrowser";
            this.PlaydarBrowser.ScriptErrorsSuppressed = true;
            this.PlaydarBrowser.Size = new System.Drawing.Size(604, 370);
            this.PlaydarBrowser.TabIndex = 0;
            this.PlaydarBrowser.WebBrowserShortcutsEnabled = false;
            this.PlaydarBrowser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.playdarBrowser_Navigating);
            this.PlaydarBrowser.NewWindow += new System.ComponentModel.CancelEventHandler(this.playdarBrowser_NewWindow);
            this.PlaydarBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.playdarBrowser_DocumentCompleted);
            // 
            // logTabPage
            // 
            this.logTabPage.Controls.Add(this.logSplitPanel);
            this.logTabPage.Location = new System.Drawing.Point(4, 25);
            this.logTabPage.Name = "logTabPage";
            this.logTabPage.Size = new System.Drawing.Size(608, 403);
            this.logTabPage.TabIndex = 4;
            this.logTabPage.Text = "Log";
            this.logTabPage.UseVisualStyleBackColor = true;
            // 
            // logSplitPanel
            // 
            this.logSplitPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logSplitPanel.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.logSplitPanel.IsSplitterFixed = true;
            this.logSplitPanel.Location = new System.Drawing.Point(0, 0);
            this.logSplitPanel.Name = "logSplitPanel";
            this.logSplitPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // logSplitPanel.Panel1
            // 
            this.logSplitPanel.Panel1.Controls.Add(this.followTailCheckBox);
            // 
            // logSplitPanel.Panel2
            // 
            this.logSplitPanel.Panel2.Controls.Add(this.logBoxPanel);
            this.logSplitPanel.Size = new System.Drawing.Size(608, 403);
            this.logSplitPanel.SplitterDistance = 25;
            this.logSplitPanel.TabIndex = 2;
            // 
            // followTailCheckBox
            // 
            this.followTailCheckBox.AutoSize = true;
            this.followTailCheckBox.Checked = true;
            this.followTailCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.followTailCheckBox.Location = new System.Drawing.Point(5, 4);
            this.followTailCheckBox.Name = "followTailCheckBox";
            this.followTailCheckBox.Size = new System.Drawing.Size(76, 17);
            this.followTailCheckBox.TabIndex = 0;
            this.followTailCheckBox.Text = "Follow Tail";
            this.followTailCheckBox.UseVisualStyleBackColor = true;
            this.followTailCheckBox.CheckedChanged += new System.EventHandler(this.followTailCheckBox_CheckedChanged);
            // 
            // logBoxPanel
            // 
            this.logBoxPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.logBoxPanel.Controls.Add(this.logBox);
            this.logBoxPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logBoxPanel.Location = new System.Drawing.Point(0, 0);
            this.logBoxPanel.Name = "logBoxPanel";
            this.logBoxPanel.Size = new System.Drawing.Size(608, 374);
            this.logBoxPanel.TabIndex = 1;
            // 
            // logBox
            // 
            this.logBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.logBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.logBox.DetectUrls = false;
            this.logBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logBox.FollowTail = true;
            this.logBox.Location = new System.Drawing.Point(0, 0);
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.ShortcutsEnabled = false;
            this.logBox.Size = new System.Drawing.Size(604, 370);
            this.logBox.TabIndex = 0;
            this.logBox.Text = "";
            this.logBox.Updating = false;
            this.logBox.WordWrap = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 442);
            this.Controls.Add(this.mainformBorderPanel);
            this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
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
            this.MainTabControl.ResumeLayout(false);
            this.aboutTabPage.ResumeLayout(false);
            this.aboutTabPage.PerformLayout();
            this.aboutCenterPanel.ResumeLayout(false);
            this.aboutCenterPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.playdarLogo)).EndInit();
            this.optionsTabPage.ResumeLayout(false);
            this.optionsTabControl.ResumeLayout(false);
            this.generalOptionsTabPage.ResumeLayout(false);
            this.generalOptionsPanel.ResumeLayout(false);
            this.aboutGroupBox.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.peersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.peersGrid)).EndInit();
            this.peersContextMenu.ResumeLayout(false);
            this.libraryTabPage.ResumeLayout(false);
            this.libraryPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.libraryGrid)).EndInit();
            this.libraryContextMenu.ResumeLayout(false);
            this.modsTabPage.ResumeLayout(false);
            this.modsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.modsGrid)).EndInit();
            this.modsContextMenu.ResumeLayout(false);
            this.pluginsTabPage.ResumeLayout(false);
            this.pluginsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.pluginsGrid)).EndInit();
            this.pluginsContextMenu.ResumeLayout(false);
            this.propsTabPage.ResumeLayout(false);
            this.propsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.propsGrid)).EndInit();
            this.propsContextMenu.ResumeLayout(false);
            this.playdarTabPage.ResumeLayout(false);
            this.daemonSplitPanel.Panel1.ResumeLayout(false);
            this.daemonSplitPanel.Panel2.ResumeLayout(false);
            this.daemonSplitPanel.ResumeLayout(false);
            this.playdarBorderPanel.ResumeLayout(false);
            this.logTabPage.ResumeLayout(false);
            this.logSplitPanel.Panel1.ResumeLayout(false);
            this.logSplitPanel.Panel1.PerformLayout();
            this.logSplitPanel.Panel2.ResumeLayout(false);
            this.logSplitPanel.ResumeLayout(false);
            this.logBoxPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel mainformBorderPanel;
        private System.Windows.Forms.TabPage aboutTabPage;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Panel aboutCenterPanel;
        private System.Windows.Forms.LinkLabel playdarLink;
        private System.Windows.Forms.PictureBox playdarLogo;
        private System.Windows.Forms.TabPage optionsTabPage;
        private System.Windows.Forms.TabPage playdarTabPage;
        private System.Windows.Forms.Panel playdarBorderPanel;
        private System.Windows.Forms.TabPage logTabPage;
        private LogTextBox logBox;
        private System.Windows.Forms.Panel logBoxPanel;
        private System.Windows.Forms.SplitContainer daemonSplitPanel;
        private System.Windows.Forms.SplitContainer logSplitPanel;
        private System.Windows.Forms.CheckBox followTailCheckBox;
        internal System.Windows.Forms.Button StopDaemonButton;
        internal System.Windows.Forms.Button StartDaemonButton;
        internal System.Windows.Forms.Button RestartDaemonButton;
        internal System.Windows.Forms.Button BackButton;
        internal System.Windows.Forms.Button HomeButton;
        private System.Windows.Forms.TabControl optionsTabControl;
        private System.Windows.Forms.Panel generalOptionsPanel;
        private System.Windows.Forms.GroupBox aboutGroupBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox portTextBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.CheckBox forwardCheckBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox allowIncomingCheckBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox peersGroupBox;
        private System.Windows.Forms.DataGridView peersGrid;
        private System.Windows.Forms.TabPage propsTabPage;
        private System.Windows.Forms.Panel propsPanel;
        private System.Windows.Forms.DataGridView propsGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn PropertyName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PropertyValue;
        private System.Windows.Forms.Button generalOptionsCancelButton;
        private System.Windows.Forms.Button generalOptionsSaveButton;
        private System.Windows.Forms.Button propsCancelButton;
        private System.Windows.Forms.Button propsSaveButton;
        private System.Windows.Forms.TabPage modsTabPage;
        private System.Windows.Forms.Panel modsPanel;
        private System.Windows.Forms.Button modsCancelButton;
        private System.Windows.Forms.Button modsSaveButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox autostartCheckBox;
        private System.Windows.Forms.TabPage pluginsTabPage;
        private System.Windows.Forms.Panel pluginsPanel;
        private System.Windows.Forms.Button pluginsCancelButton;
        private System.Windows.Forms.Button pluginsSaveButton;
        internal System.Windows.Forms.TabControl MainTabControl;
        internal System.Windows.Forms.WebBrowser PlaydarBrowser;
        private System.Windows.Forms.TabPage generalOptionsTabPage;
        private System.Windows.Forms.TextBox nodeNameTextBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label7;
        internal System.Windows.Forms.Button RefreshButton;
        private System.Windows.Forms.DataGridView modsGrid;
        private System.Windows.Forms.TabPage libraryTabPage;
        private System.Windows.Forms.Panel libraryPanel;
        private System.Windows.Forms.DataGridView libraryGrid;
        private System.Windows.Forms.Button libraryCancelButton;
        private System.Windows.Forms.Button librarySaveButton;
        private System.Windows.Forms.DataGridView pluginsGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn LibraryItemPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModuleName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModuleWeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModuleTargetTime;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ModuleLocalOnly;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ModuleEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn PluginName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn PluginEnabled;
        private System.Windows.Forms.ContextMenuStrip peersContextMenu;
        private System.Windows.Forms.ToolStripMenuItem addPeerMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removePeerMenuItem;
        private System.Windows.Forms.ContextMenuStrip libraryContextMenu;
        private System.Windows.Forms.ToolStripMenuItem addLibPathMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeLibPathMenuItem;
        private System.Windows.Forms.ContextMenuStrip modsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem addModMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeModMenuItem;
        private System.Windows.Forms.ContextMenuStrip pluginsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem addPluginMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removePluginMenuItem;
        private System.Windows.Forms.ContextMenuStrip propsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem addPropMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removePropMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn peerAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn peerPort;
        private System.Windows.Forms.DataGridViewCheckBoxColumn peerShare;
    }
}