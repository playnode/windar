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
            this.mainformTabControl = new System.Windows.Forms.TabControl();
            this.aboutTabPage = new System.Windows.Forms.TabPage();
            this.versionLabel = new System.Windows.Forms.Label();
            this.aboutCenterPanel = new System.Windows.Forms.Panel();
            this.playdarLink = new System.Windows.Forms.LinkLabel();
            this.playdarInfoBox = new System.Windows.Forms.GroupBox();
            this.playdarInfo = new System.Windows.Forms.RichTextBox();
            this.playdarLogo = new System.Windows.Forms.PictureBox();
            this.libraryTabPage = new System.Windows.Forms.TabPage();
            this.librarySplitContainer = new System.Windows.Forms.SplitContainer();
            this.libraryListBox = new System.Windows.Forms.ListBox();
            this.addButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.optionsTabPage = new System.Windows.Forms.TabPage();
            this.propsGroupBox = new System.Windows.Forms.GroupBox();
            this.propsGrid = new System.Windows.Forms.DataGridView();
            this.PropertyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PropertyValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cancelChangesButton = new System.Windows.Forms.Button();
            this.applyChangesButton = new System.Windows.Forms.Button();
            this.peersGroupBox = new System.Windows.Forms.GroupBox();
            this.peersGrid = new System.Windows.Forms.DataGridView();
            this.peerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.peerAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.peerPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.peerShare = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.peerActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.aboutGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.nodeNameTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.allowIncomingCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.playdarTabPage = new System.Windows.Forms.TabPage();
            this.daemonSplitPanel = new System.Windows.Forms.SplitContainer();
            this.restartDaemonButton = new System.Windows.Forms.Button();
            this.backButton = new System.Windows.Forms.Button();
            this.homeButton = new System.Windows.Forms.Button();
            this.stopDaemonButton = new System.Windows.Forms.Button();
            this.startDaemonButton = new System.Windows.Forms.Button();
            this.playdarBorderPanel = new System.Windows.Forms.Panel();
            this.playdarBrowser = new System.Windows.Forms.WebBrowser();
            this.logTabPage = new System.Windows.Forms.TabPage();
            this.logSplitPanel = new System.Windows.Forms.SplitContainer();
            this.followTailCheckBox = new System.Windows.Forms.CheckBox();
            this.logBoxPanel = new System.Windows.Forms.Panel();
            this.logBox = new Windar.TrayApp.LogTextBox();
            this.mainformBorderPanel.SuspendLayout();
            this.mainformTabControl.SuspendLayout();
            this.aboutTabPage.SuspendLayout();
            this.aboutCenterPanel.SuspendLayout();
            this.playdarInfoBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.playdarLogo)).BeginInit();
            this.libraryTabPage.SuspendLayout();
            this.librarySplitContainer.Panel1.SuspendLayout();
            this.librarySplitContainer.Panel2.SuspendLayout();
            this.librarySplitContainer.SuspendLayout();
            this.optionsTabPage.SuspendLayout();
            this.propsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.propsGrid)).BeginInit();
            this.peersGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.peersGrid)).BeginInit();
            this.aboutGroupBox.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
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
            this.mainformTabControl.Controls.Add(this.optionsTabPage);
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
            this.aboutCenterPanel.Controls.Add(this.playdarLink);
            this.aboutCenterPanel.Controls.Add(this.playdarInfoBox);
            this.aboutCenterPanel.Controls.Add(this.playdarLogo);
            this.aboutCenterPanel.Location = new System.Drawing.Point(13, 50);
            this.aboutCenterPanel.Name = "aboutCenterPanel";
            this.aboutCenterPanel.Size = new System.Drawing.Size(577, 274);
            this.aboutCenterPanel.TabIndex = 4;
            // 
            // playdarLink
            // 
            this.playdarLink.ActiveLinkColor = System.Drawing.Color.FromArgb(((int) (((byte) (128)))), ((int) (((byte) (128)))), ((int) (((byte) (255)))));
            this.playdarLink.AutoSize = true;
            this.playdarLink.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
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
            this.playdarInfo.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
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
            this.playdarLogo.Image = ((System.Drawing.Image) (resources.GetObject("playdarLogo.Image")));
            this.playdarLogo.Location = new System.Drawing.Point(3, 9);
            this.playdarLogo.Name = "playdarLogo";
            this.playdarLogo.Size = new System.Drawing.Size(257, 256);
            this.playdarLogo.TabIndex = 0;
            this.playdarLogo.TabStop = false;
            // 
            // libraryTabPage
            // 
            this.libraryTabPage.Controls.Add(this.librarySplitContainer);
            this.libraryTabPage.Location = new System.Drawing.Point(4, 25);
            this.libraryTabPage.Name = "libraryTabPage";
            this.libraryTabPage.Size = new System.Drawing.Size(608, 403);
            this.libraryTabPage.TabIndex = 2;
            this.libraryTabPage.Text = "Library";
            this.libraryTabPage.UseVisualStyleBackColor = true;
            // 
            // librarySplitContainer
            // 
            this.librarySplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.librarySplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.librarySplitContainer.IsSplitterFixed = true;
            this.librarySplitContainer.Location = new System.Drawing.Point(0, 0);
            this.librarySplitContainer.Name = "librarySplitContainer";
            this.librarySplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // librarySplitContainer.Panel1
            // 
            this.librarySplitContainer.Panel1.Controls.Add(this.libraryListBox);
            // 
            // librarySplitContainer.Panel2
            // 
            this.librarySplitContainer.Panel2.Controls.Add(this.addButton);
            this.librarySplitContainer.Panel2.Controls.Add(this.button1);
            this.librarySplitContainer.Panel2.Controls.Add(this.removeButton);
            this.librarySplitContainer.Size = new System.Drawing.Size(608, 403);
            this.librarySplitContainer.SplitterDistance = 370;
            this.librarySplitContainer.TabIndex = 4;
            // 
            // libraryListBox
            // 
            this.libraryListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.libraryListBox.FormattingEnabled = true;
            this.libraryListBox.Location = new System.Drawing.Point(0, 0);
            this.libraryListBox.Name = "libraryListBox";
            this.libraryListBox.Size = new System.Drawing.Size(608, 368);
            this.libraryListBox.Sorted = true;
            this.libraryListBox.TabIndex = 3;
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Location = new System.Drawing.Point(0, 3);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(23, 23);
            this.addButton.TabIndex = 0;
            this.addButton.Text = "+";
            this.addButton.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(534, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Re-index";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // removeButton
            // 
            this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeButton.Location = new System.Drawing.Point(29, 3);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(23, 23);
            this.removeButton.TabIndex = 1;
            this.removeButton.Text = "-";
            this.removeButton.UseVisualStyleBackColor = true;
            // 
            // optionsTabPage
            // 
            this.optionsTabPage.Controls.Add(this.propsGroupBox);
            this.optionsTabPage.Controls.Add(this.cancelChangesButton);
            this.optionsTabPage.Controls.Add(this.applyChangesButton);
            this.optionsTabPage.Controls.Add(this.peersGroupBox);
            this.optionsTabPage.Controls.Add(this.aboutGroupBox);
            this.optionsTabPage.Location = new System.Drawing.Point(4, 25);
            this.optionsTabPage.Name = "optionsTabPage";
            this.optionsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.optionsTabPage.Size = new System.Drawing.Size(608, 403);
            this.optionsTabPage.TabIndex = 3;
            this.optionsTabPage.Text = "Options";
            this.optionsTabPage.UseVisualStyleBackColor = true;
            // 
            // propsGroupBox
            // 
            this.propsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propsGroupBox.Controls.Add(this.propsGrid);
            this.propsGroupBox.Location = new System.Drawing.Point(352, 6);
            this.propsGroupBox.Name = "propsGroupBox";
            this.propsGroupBox.Size = new System.Drawing.Size(250, 117);
            this.propsGroupBox.TabIndex = 16;
            this.propsGroupBox.TabStop = false;
            this.propsGroupBox.Text = "Properties";
            // 
            // propsGrid
            // 
            this.propsGrid.AllowUserToAddRows = false;
            this.propsGrid.AllowUserToDeleteRows = false;
            this.propsGrid.AllowUserToResizeRows = false;
            this.propsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.propsGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.propsGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.propsGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.propsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.propsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PropertyName,
            this.PropertyValue});
            this.propsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propsGrid.Location = new System.Drawing.Point(3, 16);
            this.propsGrid.MultiSelect = false;
            this.propsGrid.Name = "propsGrid";
            this.propsGrid.RowHeadersVisible = false;
            this.propsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.propsGrid.Size = new System.Drawing.Size(244, 98);
            this.propsGrid.TabIndex = 0;
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
            // cancelChangesButton
            // 
            this.cancelChangesButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelChangesButton.Enabled = false;
            this.cancelChangesButton.Location = new System.Drawing.Point(72, 377);
            this.cancelChangesButton.Name = "cancelChangesButton";
            this.cancelChangesButton.Size = new System.Drawing.Size(60, 23);
            this.cancelChangesButton.TabIndex = 15;
            this.cancelChangesButton.Text = "Cancel";
            this.cancelChangesButton.UseVisualStyleBackColor = true;
            // 
            // applyChangesButton
            // 
            this.applyChangesButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.applyChangesButton.Location = new System.Drawing.Point(6, 377);
            this.applyChangesButton.Name = "applyChangesButton";
            this.applyChangesButton.Size = new System.Drawing.Size(60, 23);
            this.applyChangesButton.TabIndex = 14;
            this.applyChangesButton.Text = "Apply";
            this.applyChangesButton.UseVisualStyleBackColor = true;
            // 
            // peersGroupBox
            // 
            this.peersGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.peersGroupBox.Controls.Add(this.peersGrid);
            this.peersGroupBox.Location = new System.Drawing.Point(6, 129);
            this.peersGroupBox.Name = "peersGroupBox";
            this.peersGroupBox.Padding = new System.Windows.Forms.Padding(7, 4, 6, 7);
            this.peersGroupBox.Size = new System.Drawing.Size(596, 242);
            this.peersGroupBox.TabIndex = 13;
            this.peersGroupBox.TabStop = false;
            this.peersGroupBox.Text = "Other Computers";
            // 
            // peersGrid
            // 
            this.peersGrid.AllowUserToResizeRows = false;
            this.peersGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.peersGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.peersGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.peersGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.peersGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.peerName,
            this.peerAddress,
            this.peerPort,
            this.peerShare,
            this.peerActive});
            this.peersGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.peersGrid.Location = new System.Drawing.Point(7, 17);
            this.peersGrid.Name = "peersGrid";
            this.peersGrid.Size = new System.Drawing.Size(583, 218);
            this.peersGrid.TabIndex = 9;
            // 
            // peerName
            // 
            this.peerName.FillWeight = 108.4388F;
            this.peerName.HeaderText = "Peer name";
            this.peerName.MaxInputLength = 20;
            this.peerName.MinimumWidth = 60;
            this.peerName.Name = "peerName";
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
            // 
            // peerShare
            // 
            this.peerShare.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.peerShare.FillWeight = 43.7529F;
            this.peerShare.HeaderText = "Sharing";
            this.peerShare.MinimumWidth = 50;
            this.peerShare.Name = "peerShare";
            this.peerShare.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.peerShare.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.peerShare.Width = 50;
            // 
            // peerActive
            // 
            this.peerActive.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.peerActive.FillWeight = 63.95939F;
            this.peerActive.HeaderText = "Active";
            this.peerActive.MinimumWidth = 50;
            this.peerActive.Name = "peerActive";
            this.peerActive.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.peerActive.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.peerActive.Width = 50;
            // 
            // aboutGroupBox
            // 
            this.aboutGroupBox.Controls.Add(this.tableLayoutPanel1);
            this.aboutGroupBox.Controls.Add(this.tableLayoutPanel2);
            this.aboutGroupBox.Location = new System.Drawing.Point(6, 6);
            this.aboutGroupBox.Name = "aboutGroupBox";
            this.aboutGroupBox.Size = new System.Drawing.Size(339, 117);
            this.aboutGroupBox.TabIndex = 11;
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
            this.tableLayoutPanel1.Controls.Add(this.textBox1, 3, 0);
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
            // textBox1
            // 
            this.textBox1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBox1.Location = new System.Drawing.Point(268, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(50, 20);
            this.textBox1.TabIndex = 5;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 87.89238F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.10762F));
            this.tableLayoutPanel2.Controls.Add(this.checkBox2, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.checkBox1, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.allowIncomingCheckBox, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(6, 49);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(229, 62);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // checkBox2
            // 
            this.checkBox2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(204, 43);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(15, 14);
            this.checkBox2.TabIndex = 9;
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(204, 23);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(168, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Forward on queries to other nodes";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(94, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Share with everyone";
            // 
            // allowIncomingCheckBox
            // 
            this.allowIncomingCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.allowIncomingCheckBox.AutoSize = true;
            this.allowIncomingCheckBox.Location = new System.Drawing.Point(204, 3);
            this.allowIncomingCheckBox.Name = "allowIncomingCheckBox";
            this.allowIncomingCheckBox.Size = new System.Drawing.Size(15, 14);
            this.allowIncomingCheckBox.TabIndex = 2;
            this.allowIncomingCheckBox.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(153, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Block all incoming connections";
            // 
            // playdarTabPage
            // 
            this.playdarTabPage.Controls.Add(this.daemonSplitPanel);
            this.playdarTabPage.Location = new System.Drawing.Point(4, 25);
            this.playdarTabPage.Name = "playdarTabPage";
            this.playdarTabPage.Size = new System.Drawing.Size(608, 403);
            this.playdarTabPage.TabIndex = 1;
            this.playdarTabPage.Text = "Playdar Core";
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
            this.daemonSplitPanel.Panel1.Controls.Add(this.restartDaemonButton);
            this.daemonSplitPanel.Panel1.Controls.Add(this.backButton);
            this.daemonSplitPanel.Panel1.Controls.Add(this.homeButton);
            this.daemonSplitPanel.Panel1.Controls.Add(this.stopDaemonButton);
            this.daemonSplitPanel.Panel1.Controls.Add(this.startDaemonButton);
            // 
            // daemonSplitPanel.Panel2
            // 
            this.daemonSplitPanel.Panel2.Controls.Add(this.playdarBorderPanel);
            this.daemonSplitPanel.Size = new System.Drawing.Size(608, 403);
            this.daemonSplitPanel.SplitterDistance = 25;
            this.daemonSplitPanel.TabIndex = 2;
            // 
            // restartDaemonButton
            // 
            this.restartDaemonButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.restartDaemonButton.Location = new System.Drawing.Point(546, 0);
            this.restartDaemonButton.Name = "restartDaemonButton";
            this.restartDaemonButton.Size = new System.Drawing.Size(60, 23);
            this.restartDaemonButton.TabIndex = 5;
            this.restartDaemonButton.Text = "Restart";
            this.restartDaemonButton.UseVisualStyleBackColor = true;
            this.restartDaemonButton.Click += new System.EventHandler(this.restartButton_Click);
            // 
            // backButton
            // 
            this.backButton.Enabled = false;
            this.backButton.Location = new System.Drawing.Point(68, 0);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(60, 23);
            this.backButton.TabIndex = 4;
            this.backButton.Text = "Back";
            this.backButton.UseVisualStyleBackColor = true;
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // homeButton
            // 
            this.homeButton.Enabled = false;
            this.homeButton.Location = new System.Drawing.Point(2, 0);
            this.homeButton.Name = "homeButton";
            this.homeButton.Size = new System.Drawing.Size(60, 23);
            this.homeButton.TabIndex = 3;
            this.homeButton.Text = "Home";
            this.homeButton.UseVisualStyleBackColor = true;
            this.homeButton.Click += new System.EventHandler(this.homeButton_Click);
            // 
            // stopDaemonButton
            // 
            this.stopDaemonButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.stopDaemonButton.Location = new System.Drawing.Point(480, 0);
            this.stopDaemonButton.Name = "stopDaemonButton";
            this.stopDaemonButton.Size = new System.Drawing.Size(60, 23);
            this.stopDaemonButton.TabIndex = 2;
            this.stopDaemonButton.Text = "Stop";
            this.stopDaemonButton.UseVisualStyleBackColor = true;
            this.stopDaemonButton.Click += new System.EventHandler(this.stopDaemonButton_Click);
            // 
            // startDaemonButton
            // 
            this.startDaemonButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.startDaemonButton.Enabled = false;
            this.startDaemonButton.Location = new System.Drawing.Point(414, 0);
            this.startDaemonButton.Name = "startDaemonButton";
            this.startDaemonButton.Size = new System.Drawing.Size(60, 23);
            this.startDaemonButton.TabIndex = 1;
            this.startDaemonButton.Text = "Start";
            this.startDaemonButton.UseVisualStyleBackColor = true;
            this.startDaemonButton.Click += new System.EventHandler(this.startDaemonButton_Click);
            // 
            // playdarBorderPanel
            // 
            this.playdarBorderPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.playdarBorderPanel.Controls.Add(this.playdarBrowser);
            this.playdarBorderPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playdarBorderPanel.Location = new System.Drawing.Point(0, 0);
            this.playdarBorderPanel.Name = "playdarBorderPanel";
            this.playdarBorderPanel.Size = new System.Drawing.Size(608, 374);
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
            this.playdarBrowser.Size = new System.Drawing.Size(604, 370);
            this.playdarBrowser.TabIndex = 0;
            this.playdarBrowser.WebBrowserShortcutsEnabled = false;
            this.playdarBrowser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.playdarBrowser_Navigating);
            this.playdarBrowser.NewWindow += new System.ComponentModel.CancelEventHandler(this.playdarBrowser_NewWindow);
            this.playdarBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.playdarBrowser_DocumentCompleted);
            // 
            // logTabPage
            // 
            this.logTabPage.Controls.Add(this.logSplitPanel);
            this.logTabPage.Location = new System.Drawing.Point(4, 25);
            this.logTabPage.Name = "logTabPage";
            this.logTabPage.Size = new System.Drawing.Size(608, 403);
            this.logTabPage.TabIndex = 4;
            this.logTabPage.Text = "Debug Log";
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
            this.mainformTabControl.ResumeLayout(false);
            this.aboutTabPage.ResumeLayout(false);
            this.aboutTabPage.PerformLayout();
            this.aboutCenterPanel.ResumeLayout(false);
            this.aboutCenterPanel.PerformLayout();
            this.playdarInfoBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.playdarLogo)).EndInit();
            this.libraryTabPage.ResumeLayout(false);
            this.librarySplitContainer.Panel1.ResumeLayout(false);
            this.librarySplitContainer.Panel2.ResumeLayout(false);
            this.librarySplitContainer.ResumeLayout(false);
            this.optionsTabPage.ResumeLayout(false);
            this.propsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.propsGrid)).EndInit();
            this.peersGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.peersGrid)).EndInit();
            this.aboutGroupBox.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
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
        private System.Windows.Forms.TabControl mainformTabControl;
        private System.Windows.Forms.TabPage aboutTabPage;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Panel aboutCenterPanel;
        private System.Windows.Forms.LinkLabel playdarLink;
        private System.Windows.Forms.GroupBox playdarInfoBox;
        private System.Windows.Forms.RichTextBox playdarInfo;
        private System.Windows.Forms.PictureBox playdarLogo;
        private System.Windows.Forms.TabPage libraryTabPage;
        private System.Windows.Forms.TabPage optionsTabPage;
        private System.Windows.Forms.TabPage playdarTabPage;
        private System.Windows.Forms.Panel playdarBorderPanel;
        private System.Windows.Forms.TabPage logTabPage;
        private LogTextBox logBox;
        private System.Windows.Forms.Panel logBoxPanel;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox libraryListBox;
        private System.Windows.Forms.SplitContainer librarySplitContainer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nodeNameTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox allowIncomingCheckBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.DataGridView peersGrid;
        private System.Windows.Forms.GroupBox aboutGroupBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn peerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn peerAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn peerPort;
        private System.Windows.Forms.DataGridViewCheckBoxColumn peerShare;
        private System.Windows.Forms.DataGridViewCheckBoxColumn peerActive;
        private System.Windows.Forms.GroupBox peersGroupBox;
        private System.Windows.Forms.SplitContainer daemonSplitPanel;
        private System.Windows.Forms.SplitContainer logSplitPanel;
        private System.Windows.Forms.CheckBox followTailCheckBox;
        private System.Windows.Forms.Button cancelChangesButton;
        private System.Windows.Forms.Button applyChangesButton;
        internal System.Windows.Forms.Button stopDaemonButton;
        internal System.Windows.Forms.Button startDaemonButton;
        internal System.Windows.Forms.Button restartDaemonButton;
        internal System.Windows.Forms.Button backButton;
        internal System.Windows.Forms.Button homeButton;
        private System.Windows.Forms.WebBrowser playdarBrowser;
        private System.Windows.Forms.GroupBox propsGroupBox;
        private System.Windows.Forms.DataGridView propsGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn PropertyName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PropertyValue;
    }
}