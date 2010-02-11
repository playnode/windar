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

namespace Windar.PlayerPlugin
{
    partial class PlayerTabPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.artistLabel = new System.Windows.Forms.Label();
            this.trackLabel = new System.Windows.Forms.Label();
            this.albumLabel = new System.Windows.Forms.Label();
            this.queryGroup = new System.Windows.Forms.GroupBox();
            this.resetButton = new System.Windows.Forms.Button();
            this.resolveButton = new System.Windows.Forms.Button();
            this.albumTextbox = new System.Windows.Forms.TextBox();
            this.trackTextbox = new System.Windows.Forms.TextBox();
            this.artistTextbox = new System.Windows.Forms.TextBox();
            this.playButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.resultsGrid = new System.Windows.Forms.DataGridView();
            this.ArtistName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TrackName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AlbumName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Source = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resultsGroupBox = new System.Windows.Forms.GroupBox();
            this.volumeTrackbar = new System.Windows.Forms.TrackBar();
            this.positionTrackbar = new System.Windows.Forms.TrackBar();
            this.volLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.statusGroupBox = new System.Windows.Forms.GroupBox();
            this.queryTimer = new System.Windows.Forms.Timer(this.components);
            this.progressTimer = new System.Windows.Forms.Timer(this.components);
            this.queryGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.resultsGrid)).BeginInit();
            this.resultsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.volumeTrackbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.positionTrackbar)).BeginInit();
            this.statusGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // artistLabel
            // 
            this.artistLabel.AutoSize = true;
            this.artistLabel.Location = new System.Drawing.Point(10, 18);
            this.artistLabel.Name = "artistLabel";
            this.artistLabel.Size = new System.Drawing.Size(30, 13);
            this.artistLabel.TabIndex = 0;
            this.artistLabel.Text = "Artist";
            // 
            // trackLabel
            // 
            this.trackLabel.AutoSize = true;
            this.trackLabel.Location = new System.Drawing.Point(143, 17);
            this.trackLabel.Name = "trackLabel";
            this.trackLabel.Size = new System.Drawing.Size(64, 13);
            this.trackLabel.TabIndex = 1;
            this.trackLabel.Text = "Track name";
            // 
            // albumLabel
            // 
            this.albumLabel.AutoSize = true;
            this.albumLabel.Location = new System.Drawing.Point(282, 17);
            this.albumLabel.Name = "albumLabel";
            this.albumLabel.Size = new System.Drawing.Size(82, 13);
            this.albumLabel.TabIndex = 2;
            this.albumLabel.Text = "Album (optional)";
            // 
            // queryGroup
            // 
            this.queryGroup.Controls.Add(this.resetButton);
            this.queryGroup.Controls.Add(this.resolveButton);
            this.queryGroup.Controls.Add(this.albumTextbox);
            this.queryGroup.Controls.Add(this.albumLabel);
            this.queryGroup.Controls.Add(this.trackTextbox);
            this.queryGroup.Controls.Add(this.trackLabel);
            this.queryGroup.Controls.Add(this.artistTextbox);
            this.queryGroup.Controls.Add(this.artistLabel);
            this.queryGroup.Location = new System.Drawing.Point(0, 3);
            this.queryGroup.Name = "queryGroup";
            this.queryGroup.Size = new System.Drawing.Size(512, 65);
            this.queryGroup.TabIndex = 3;
            this.queryGroup.TabStop = false;
            this.queryGroup.Text = "Query";
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(455, 32);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(48, 23);
            this.resetButton.TabIndex = 5;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // resolveButton
            // 
            this.resolveButton.Location = new System.Drawing.Point(418, 32);
            this.resolveButton.Name = "resolveButton";
            this.resolveButton.Size = new System.Drawing.Size(33, 23);
            this.resolveButton.TabIndex = 4;
            this.resolveButton.Text = "Go";
            this.resolveButton.UseVisualStyleBackColor = true;
            this.resolveButton.Click += new System.EventHandler(this.resolveButton_Click);
            // 
            // albumTextbox
            // 
            this.albumTextbox.AcceptsReturn = true;
            this.albumTextbox.Location = new System.Drawing.Point(282, 34);
            this.albumTextbox.Name = "albumTextbox";
            this.albumTextbox.Size = new System.Drawing.Size(130, 20);
            this.albumTextbox.TabIndex = 3;
            this.albumTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.albumTextbox_KeyDown);
            this.albumTextbox.Enter += new System.EventHandler(this.albumTextbox_Enter);
            // 
            // trackTextbox
            // 
            this.trackTextbox.AcceptsReturn = true;
            this.trackTextbox.Location = new System.Drawing.Point(146, 34);
            this.trackTextbox.Name = "trackTextbox";
            this.trackTextbox.Size = new System.Drawing.Size(130, 20);
            this.trackTextbox.TabIndex = 2;
            this.trackTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.trackTextbox_KeyDown);
            this.trackTextbox.Enter += new System.EventHandler(this.trackTextbox_Enter);
            // 
            // artistTextbox
            // 
            this.artistTextbox.AcceptsReturn = true;
            this.artistTextbox.Location = new System.Drawing.Point(10, 34);
            this.artistTextbox.Name = "artistTextbox";
            this.artistTextbox.Size = new System.Drawing.Size(130, 20);
            this.artistTextbox.TabIndex = 1;
            this.artistTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.artistTextbox_KeyDown);
            this.artistTextbox.Enter += new System.EventHandler(this.artistTextbox_Enter);
            // 
            // playButton
            // 
            this.playButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.playButton.Location = new System.Drawing.Point(0, 369);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(78, 23);
            this.playButton.TabIndex = 4;
            this.playButton.Text = "Play/Pause";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.stopButton.Location = new System.Drawing.Point(84, 369);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(46, 23);
            this.stopButton.TabIndex = 5;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // resultsGrid
            // 
            this.resultsGrid.AllowUserToAddRows = false;
            this.resultsGrid.AllowUserToDeleteRows = false;
            this.resultsGrid.AllowUserToResizeRows = false;
            this.resultsGrid.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.resultsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.resultsGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.resultsGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.resultsGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.resultsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resultsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ArtistName,
            this.TrackName,
            this.AlbumName,
            this.Source});
            this.resultsGrid.Location = new System.Drawing.Point(9, 19);
            this.resultsGrid.MultiSelect = false;
            this.resultsGrid.Name = "resultsGrid";
            this.resultsGrid.ReadOnly = true;
            this.resultsGrid.RowHeadersVisible = false;
            this.resultsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.resultsGrid.Size = new System.Drawing.Size(582, 219);
            this.resultsGrid.StandardTab = true;
            this.resultsGrid.TabIndex = 27;
            this.resultsGrid.MouseClick += new System.Windows.Forms.MouseEventHandler(this.resultsGrid_MouseClick);
            this.resultsGrid.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.resultsGrid_CellMouseDoubleClick);
            // 
            // ArtistName
            // 
            this.ArtistName.HeaderText = "Artist";
            this.ArtistName.MaxInputLength = 255;
            this.ArtistName.Name = "ArtistName";
            this.ArtistName.ReadOnly = true;
            // 
            // TrackName
            // 
            this.TrackName.HeaderText = "Track";
            this.TrackName.MaxInputLength = 255;
            this.TrackName.Name = "TrackName";
            this.TrackName.ReadOnly = true;
            // 
            // AlbumName
            // 
            this.AlbumName.HeaderText = "Album";
            this.AlbumName.MaxInputLength = 255;
            this.AlbumName.Name = "AlbumName";
            this.AlbumName.ReadOnly = true;
            // 
            // Source
            // 
            this.Source.HeaderText = "Source";
            this.Source.MaxInputLength = 255;
            this.Source.Name = "Source";
            this.Source.ReadOnly = true;
            // 
            // resultsGroupBox
            // 
            this.resultsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.resultsGroupBox.Controls.Add(this.resultsGrid);
            this.resultsGroupBox.Location = new System.Drawing.Point(0, 74);
            this.resultsGroupBox.Name = "resultsGroupBox";
            this.resultsGroupBox.Size = new System.Drawing.Size(600, 250);
            this.resultsGroupBox.TabIndex = 28;
            this.resultsGroupBox.TabStop = false;
            this.resultsGroupBox.Text = "Results";
            // 
            // volumeTrackbar
            // 
            this.volumeTrackbar.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.volumeTrackbar.AutoSize = false;
            this.volumeTrackbar.Location = new System.Drawing.Point(167, 369);
            this.volumeTrackbar.Maximum = 100;
            this.volumeTrackbar.Name = "volumeTrackbar";
            this.volumeTrackbar.Size = new System.Drawing.Size(90, 23);
            this.volumeTrackbar.TabIndex = 29;
            this.volumeTrackbar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.volumeTrackbar.Value = 100;
            this.volumeTrackbar.Scroll += new System.EventHandler(this.volumeTrackbar_Scroll);
            // 
            // positionTrackbar
            // 
            this.positionTrackbar.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.positionTrackbar.AutoSize = false;
            this.positionTrackbar.Location = new System.Drawing.Point(297, 369);
            this.positionTrackbar.Maximum = 100;
            this.positionTrackbar.Name = "positionTrackbar";
            this.positionTrackbar.Size = new System.Drawing.Size(303, 23);
            this.positionTrackbar.TabIndex = 30;
            this.positionTrackbar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.positionTrackbar.Scroll += new System.EventHandler(this.positionTrackbar_Scroll);
            // 
            // volLabel
            // 
            this.volLabel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.volLabel.AutoSize = true;
            this.volLabel.Location = new System.Drawing.Point(136, 374);
            this.volLabel.Name = "volLabel";
            this.volLabel.Size = new System.Drawing.Size(25, 13);
            this.volLabel.TabIndex = 31;
            this.volLabel.Text = "Vol:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(263, 374);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 32;
            this.label4.Text = "Pos:";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.statusLabel.Location = new System.Drawing.Point(5, 15);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 13);
            this.statusLabel.TabIndex = 33;
            // 
            // statusGroupBox
            // 
            this.statusGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.statusGroupBox.Controls.Add(this.statusLabel);
            this.statusGroupBox.Location = new System.Drawing.Point(0, 325);
            this.statusGroupBox.Name = "statusGroupBox";
            this.statusGroupBox.Size = new System.Drawing.Size(600, 38);
            this.statusGroupBox.TabIndex = 33;
            this.statusGroupBox.TabStop = false;
            // 
            // queryTimer
            // 
            this.queryTimer.Interval = 1000;
            this.queryTimer.Tick += new System.EventHandler(this.queryTimer_Tick);
            // 
            // progressTimer
            // 
            this.progressTimer.Interval = 2000;
            this.progressTimer.Tick += new System.EventHandler(this.progressTimer_Tick);
            // 
            // PlayerTabPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statusGroupBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.volLabel);
            this.Controls.Add(this.positionTrackbar);
            this.Controls.Add(this.volumeTrackbar);
            this.Controls.Add(this.resultsGroupBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.playButton);
            this.Controls.Add(this.queryGroup);
            this.Name = "PlayerTabPage";
            this.Size = new System.Drawing.Size(600, 392);
            this.Load += new System.EventHandler(this.PlayerTabPage_Load);
            this.queryGroup.ResumeLayout(false);
            this.queryGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.resultsGrid)).EndInit();
            this.resultsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.volumeTrackbar)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.positionTrackbar)).EndInit();
            this.statusGroupBox.ResumeLayout(false);
            this.statusGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label artistLabel;
        private System.Windows.Forms.Label trackLabel;
        private System.Windows.Forms.Label albumLabel;
        private System.Windows.Forms.GroupBox queryGroup;
        private System.Windows.Forms.TextBox albumTextbox;
        private System.Windows.Forms.TextBox trackTextbox;
        private System.Windows.Forms.TextBox artistTextbox;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.DataGridView resultsGrid;
        private System.Windows.Forms.GroupBox resultsGroupBox;
        private System.Windows.Forms.TrackBar volumeTrackbar;
        private System.Windows.Forms.TrackBar positionTrackbar;
        private System.Windows.Forms.Label volLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button resolveButton;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.GroupBox statusGroupBox;
        private System.Windows.Forms.Timer queryTimer;
        private System.Windows.Forms.DataGridViewTextBoxColumn ArtistName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TrackName;
        private System.Windows.Forms.DataGridViewTextBoxColumn AlbumName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Source;
        private System.Windows.Forms.Timer progressTimer;

    }
}
