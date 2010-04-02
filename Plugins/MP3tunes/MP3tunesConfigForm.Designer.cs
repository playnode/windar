/*************************************************************************
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * Copyright (C) 2009, 2010 Steven Robertson <steve@playnode.org>
 *
 * Windar: Playdar for Windows
 *
 * Windar is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 2 of the License, or
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
            this.mp3tunesAbout = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(-3, 68);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(53, 13);
            this.passwordLabel.TabIndex = 5;
            this.passwordLabel.Text = "Password";
            // 
            // usernameLabel
            // 
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Location = new System.Drawing.Point(-3, 45);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(55, 13);
            this.usernameLabel.TabIndex = 3;
            this.usernameLabel.Text = "Username";
            // 
            // passwordTextbox
            // 
            this.passwordTextbox.Location = new System.Drawing.Point(58, 65);
            this.passwordTextbox.MaxLength = 255;
            this.passwordTextbox.Name = "passwordTextbox";
            this.passwordTextbox.Size = new System.Drawing.Size(177, 20);
            this.passwordTextbox.TabIndex = 6;
            this.passwordTextbox.UseSystemPasswordChar = true;
            this.passwordTextbox.TextChanged += new System.EventHandler(this.passwordTextbox_TextChanged);
            this.passwordTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.passwordTextbox_KeyDown);
            this.passwordTextbox.Enter += new System.EventHandler(this.passwordTextbox_Enter);
            // 
            // usernameTextbox
            // 
            this.usernameTextbox.Location = new System.Drawing.Point(58, 42);
            this.usernameTextbox.MaxLength = 255;
            this.usernameTextbox.Name = "usernameTextbox";
            this.usernameTextbox.Size = new System.Drawing.Size(177, 20);
            this.usernameTextbox.TabIndex = 4;
            this.usernameTextbox.TextChanged += new System.EventHandler(this.usernameTextbox_TextChanged);
            this.usernameTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.usernameTextbox_KeyDown);
            this.usernameTextbox.Enter += new System.EventHandler(this.usernameTextbox_Enter);
            // 
            // mp3tunesLink
            // 
            this.mp3tunesLink.AutoSize = true;
            this.mp3tunesLink.Location = new System.Drawing.Point(-3, 13);
            this.mp3tunesLink.Name = "mp3tunesLink";
            this.mp3tunesLink.Size = new System.Drawing.Size(103, 13);
            this.mp3tunesLink.TabIndex = 2;
            this.mp3tunesLink.TabStop = true;
            this.mp3tunesLink.Text = "www.mp3tunes.com";
            this.mp3tunesLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.mp3tunesLink_LinkClicked);
            // 
            // mp3tunesAbout
            // 
            this.mp3tunesAbout.AutoSize = true;
            this.mp3tunesAbout.Location = new System.Drawing.Point(-3, 0);
            this.mp3tunesAbout.Name = "mp3tunesAbout";
            this.mp3tunesAbout.Size = new System.Drawing.Size(434, 13);
            this.mp3tunesAbout.TabIndex = 1;
            this.mp3tunesAbout.Text = "Resolve and stream tracks with a personal (free or paid) MP3tunes online storage " +
                "account.";
            // 
            // MP3tunesConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mp3tunesAbout);
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
        System.Windows.Forms.Label mp3tunesAbout;
    }
}
