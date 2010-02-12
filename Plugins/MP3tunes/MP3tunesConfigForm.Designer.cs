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

namespace Windar.MP3tunes
{
    partial class MP3tunesConfigForm
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
            this.passwordLabel = new System.Windows.Forms.Label();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.passwordTextbox = new System.Windows.Forms.TextBox();
            this.usernameTextbox = new System.Windows.Forms.TextBox();
            this.mp3tunesLink = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.tokenLabel = new System.Windows.Forms.Label();
            this.tokenTextbox = new System.Windows.Forms.TextBox();
            this.tokenLink = new System.Windows.Forms.LinkLabel();
            this.tokenInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(4, 72);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(53, 13);
            this.passwordLabel.TabIndex = 11;
            this.passwordLabel.Text = "Password";
            // 
            // usernameLabel
            // 
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Location = new System.Drawing.Point(4, 49);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(55, 13);
            this.usernameLabel.TabIndex = 10;
            this.usernameLabel.Text = "Username";
            // 
            // passwordTextbox
            // 
            this.passwordTextbox.Location = new System.Drawing.Point(65, 69);
            this.passwordTextbox.MaxLength = 255;
            this.passwordTextbox.Name = "passwordTextbox";
            this.passwordTextbox.Size = new System.Drawing.Size(177, 20);
            this.passwordTextbox.TabIndex = 9;
            this.passwordTextbox.UseSystemPasswordChar = true;
            this.passwordTextbox.TextChanged += new System.EventHandler(this.passwordTextbox_TextChanged);
            // 
            // usernameTextbox
            // 
            this.usernameTextbox.Location = new System.Drawing.Point(65, 46);
            this.usernameTextbox.MaxLength = 255;
            this.usernameTextbox.Name = "usernameTextbox";
            this.usernameTextbox.Size = new System.Drawing.Size(177, 20);
            this.usernameTextbox.TabIndex = 8;
            this.usernameTextbox.TextChanged += new System.EventHandler(this.usernameTextbox_TextChanged);
            // 
            // mp3tunesLink
            // 
            this.mp3tunesLink.AutoSize = true;
            this.mp3tunesLink.Location = new System.Drawing.Point(4, 17);
            this.mp3tunesLink.Name = "mp3tunesLink";
            this.mp3tunesLink.Size = new System.Drawing.Size(103, 13);
            this.mp3tunesLink.TabIndex = 13;
            this.mp3tunesLink.TabStop = true;
            this.mp3tunesLink.Text = "www.mp3tunes.com";
            this.mp3tunesLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.mp3tunesLink_LinkClicked);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(466, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Resolve and stream tracks with MP3tunes. Requires a free or paid account with dev" +
                "eloper token.";
            // 
            // tokenLabel
            // 
            this.tokenLabel.AutoSize = true;
            this.tokenLabel.Location = new System.Drawing.Point(4, 95);
            this.tokenLabel.Name = "tokenLabel";
            this.tokenLabel.Size = new System.Drawing.Size(38, 13);
            this.tokenLabel.TabIndex = 16;
            this.tokenLabel.Text = "Token";
            // 
            // tokenTextbox
            // 
            this.tokenTextbox.Location = new System.Drawing.Point(65, 92);
            this.tokenTextbox.MaxLength = 10;
            this.tokenTextbox.Name = "tokenTextbox";
            this.tokenTextbox.Size = new System.Drawing.Size(177, 20);
            this.tokenTextbox.TabIndex = 15;
            this.tokenTextbox.TextChanged += new System.EventHandler(this.tokenTextbox_TextChanged);
            // 
            // tokenLink
            // 
            this.tokenLink.AutoSize = true;
            this.tokenLink.Location = new System.Drawing.Point(248, 108);
            this.tokenLink.Name = "tokenLink";
            this.tokenLink.Size = new System.Drawing.Size(195, 13);
            this.tokenLink.TabIndex = 17;
            this.tokenLink.TabStop = true;
            this.tokenLink.Text = "www.mp3tunes.com/partner/cb/tokens";
            this.tokenLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.tokensLink_LinkClicked);
            // 
            // tokenInfo
            // 
            this.tokenInfo.AutoSize = true;
            this.tokenInfo.Location = new System.Drawing.Point(248, 95);
            this.tokenInfo.Name = "tokenInfo";
            this.tokenInfo.Size = new System.Drawing.Size(256, 13);
            this.tokenInfo.TabIndex = 18;
            this.tokenInfo.Text = "The default partner token 9999999999 may be used.";
            // 
            // MP3tunesConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tokenInfo);
            this.Controls.Add(this.tokenLink);
            this.Controls.Add(this.tokenLabel);
            this.Controls.Add(this.tokenTextbox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.mp3tunesLink);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.usernameLabel);
            this.Controls.Add(this.passwordTextbox);
            this.Controls.Add(this.usernameTextbox);
            this.Name = "MP3tunesConfigForm";
            this.Size = new System.Drawing.Size(600, 392);
            this.Load += new System.EventHandler(this.MP3tunesConfigForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        System.Windows.Forms.Label passwordLabel;
        System.Windows.Forms.Label usernameLabel;
        System.Windows.Forms.TextBox passwordTextbox;
        System.Windows.Forms.TextBox usernameTextbox;
        System.Windows.Forms.LinkLabel mp3tunesLink;
        System.Windows.Forms.Label label3;
        System.Windows.Forms.Label tokenLabel;
        System.Windows.Forms.TextBox tokenTextbox;
        System.Windows.Forms.LinkLabel tokenLink;
        System.Windows.Forms.Label tokenInfo;
    }
}
