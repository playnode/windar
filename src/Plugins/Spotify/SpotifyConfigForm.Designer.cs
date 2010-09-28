/*
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * Copyright (C) 2009, 2010 Steven Robertson <steve@playnode.org>
 *
 * Windar - Playdar for Windows
 *
 * Windar is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License (LGPL) as published
 * by the Free Software Foundation; either version 2.1 of the License, or (at
 * your option) any later version.
 *
 * Windar is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License version 2.1 for more details
 * (a copy is included in the LICENSE file that accompanied this code).
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

namespace Windar.SpotifyPlugin
{
    partial class SpotifyConfigForm
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
            this.spotifyLink = new System.Windows.Forms.LinkLabel();
            this.spotifyNote = new System.Windows.Forms.Label();
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
            this.passwordTextbox.TextChanged += new System.EventHandler(this.password_TextChanged);
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
            this.usernameTextbox.TextChanged += new System.EventHandler(this.username_TextChanged);
            this.usernameTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.usernameTextbox_KeyDown);
            this.usernameTextbox.Enter += new System.EventHandler(this.usernameTextbox_Enter);
            // 
            // spotifyLink
            // 
            this.spotifyLink.AutoSize = true;
            this.spotifyLink.Location = new System.Drawing.Point(-3, 13);
            this.spotifyLink.Name = "spotifyLink";
            this.spotifyLink.Size = new System.Drawing.Size(92, 13);
            this.spotifyLink.TabIndex = 2;
            this.spotifyLink.TabStop = true;
            this.spotifyLink.Text = "www.spotify.com";
            this.spotifyLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.spotifyLink_LinkClicked);
            // 
            // spotifyNote
            // 
            this.spotifyNote.AutoSize = true;
            this.spotifyNote.Location = new System.Drawing.Point(-3, 0);
            this.spotifyNote.Name = "spotifyNote";
            this.spotifyNote.Size = new System.Drawing.Size(439, 13);
            this.spotifyNote.TabIndex = 1;
            this.spotifyNote.Text = "Resolver for Spotify. Requires a Spotify Premium account.";
            // 
            // SpotifyConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.spotifyNote);
            this.Controls.Add(this.spotifyLink);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.usernameLabel);
            this.Controls.Add(this.passwordTextbox);
            this.Controls.Add(this.usernameTextbox);
            this.Name = "SpotifyConfigForm";
            this.Size = new System.Drawing.Size(600, 392);
            this.Load += new System.EventHandler(this.SpotifyConfigForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        System.Windows.Forms.Label passwordLabel;
        System.Windows.Forms.Label usernameLabel;
        System.Windows.Forms.TextBox passwordTextbox;
        System.Windows.Forms.TextBox usernameTextbox;
        System.Windows.Forms.LinkLabel spotifyLink;
        System.Windows.Forms.Label spotifyNote;

    }
}
