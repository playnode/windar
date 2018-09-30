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
            this.spotifyText = new System.Windows.Forms.TextBox();
            this.spotifyCoreLogo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize) (this.spotifyCoreLogo)).BeginInit();
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
            this.passwordTextbox.Enter += new System.EventHandler(this.passwordTextbox_Enter);
            this.passwordTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.passwordTextbox_KeyDown);
            // 
            // usernameTextbox
            // 
            this.usernameTextbox.Location = new System.Drawing.Point(58, 42);
            this.usernameTextbox.MaxLength = 255;
            this.usernameTextbox.Name = "usernameTextbox";
            this.usernameTextbox.Size = new System.Drawing.Size(177, 20);
            this.usernameTextbox.TabIndex = 4;
            this.usernameTextbox.TextChanged += new System.EventHandler(this.username_TextChanged);
            this.usernameTextbox.Enter += new System.EventHandler(this.usernameTextbox_Enter);
            this.usernameTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.usernameTextbox_KeyDown);
            // 
            // spotifyLink
            // 
            this.spotifyLink.AutoSize = true;
            this.spotifyLink.Location = new System.Drawing.Point(-3, 13);
            this.spotifyLink.Name = "spotifyLink";
            this.spotifyLink.Size = new System.Drawing.Size(87, 13);
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
            this.spotifyNote.Size = new System.Drawing.Size(279, 13);
            this.spotifyNote.TabIndex = 1;
            this.spotifyNote.Text = "Resolver for Spotify. Requires a Spotify Premium account.";
            // 
            // spotifyText
            // 
            this.spotifyText.BackColor = System.Drawing.SystemColors.Control;
            this.spotifyText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.spotifyText.Location = new System.Drawing.Point(185, 186);
            this.spotifyText.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.spotifyText.Multiline = true;
            this.spotifyText.Name = "spotifyText";
            this.spotifyText.Size = new System.Drawing.Size(277, 85);
            this.spotifyText.TabIndex = 7;
            this.spotifyText.Text = "This product uses SPOTIFY(R) CORE but is not endorsed, certified or otherwise app" +
                "roved in any way by Spotify. Spotify is the registered trade mark of the Spotify" +
                " Group.";
            // 
            // spotifyCoreLogo
            // 
            this.spotifyCoreLogo.Image = global::Windar.SpotifyPlugin.Properties.Resources.spotify_core_logo_128x128;
            this.spotifyCoreLogo.Location = new System.Drawing.Point(45, 143);
            this.spotifyCoreLogo.Name = "spotifyCoreLogo";
            this.spotifyCoreLogo.Size = new System.Drawing.Size(128, 128);
            this.spotifyCoreLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.spotifyCoreLogo.TabIndex = 8;
            this.spotifyCoreLogo.TabStop = false;
            // 
            // SpotifyConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.spotifyCoreLogo);
            this.Controls.Add(this.spotifyText);
            this.Controls.Add(this.spotifyNote);
            this.Controls.Add(this.spotifyLink);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.usernameLabel);
            this.Controls.Add(this.passwordTextbox);
            this.Controls.Add(this.usernameTextbox);
            this.Name = "SpotifyConfigForm";
            this.Size = new System.Drawing.Size(571, 317);
            this.Load += new System.EventHandler(this.SpotifyConfigForm_Load);
            ((System.ComponentModel.ISupportInitialize) (this.spotifyCoreLogo)).EndInit();
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
        private System.Windows.Forms.TextBox spotifyText;
        private System.Windows.Forms.PictureBox spotifyCoreLogo;

    }
}
