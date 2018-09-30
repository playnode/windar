namespace Windar.NapsterPlugin
{
    partial class NapsterConfigForm
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
            this.napsterLink = new System.Windows.Forms.LinkLabel();
            this.napsterNote = new System.Windows.Forms.Label();
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
            // napsterLink
            // 
            this.napsterLink.AutoSize = true;
            this.napsterLink.Location = new System.Drawing.Point(-3, 13);
            this.napsterLink.Name = "napsterLink";
            this.napsterLink.Size = new System.Drawing.Size(92, 13);
            this.napsterLink.TabIndex = 2;
            this.napsterLink.TabStop = true;
            this.napsterLink.Text = "www.napster.com";
            this.napsterLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.napsterLink_LinkClicked);
            // 
            // napsterNote
            // 
            this.napsterNote.AutoSize = true;
            this.napsterNote.Location = new System.Drawing.Point(-3, 0);
            this.napsterNote.Name = "napsterNote";
            this.napsterNote.Size = new System.Drawing.Size(439, 13);
            this.napsterNote.TabIndex = 1;
            this.napsterNote.Text = "Resolver for Napster. Paid account has full streams, otherwise provides 30 second" +
                " samples.";
            // 
            // NapsterConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.napsterNote);
            this.Controls.Add(this.napsterLink);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.usernameLabel);
            this.Controls.Add(this.passwordTextbox);
            this.Controls.Add(this.usernameTextbox);
            this.Name = "NapsterConfigForm";
            this.Size = new System.Drawing.Size(600, 392);
            this.Load += new System.EventHandler(this.NapsterConfigForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        System.Windows.Forms.Label passwordLabel;
        System.Windows.Forms.Label usernameLabel;
        System.Windows.Forms.TextBox passwordTextbox;
        System.Windows.Forms.TextBox usernameTextbox;
        System.Windows.Forms.LinkLabel napsterLink;
        System.Windows.Forms.Label napsterNote;

    }
}
