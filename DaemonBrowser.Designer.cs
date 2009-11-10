/*
 * Windar: Playdar for Windows
 * Copyright (C) 2009 Steven Robertson <steve.r@k-os.net>
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

namespace Windar
{
    partial class DaemonBrowser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DaemonBrowser));
            this.DaemonWebBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // DaemonWebBrowser
            // 
            this.DaemonWebBrowser.AllowWebBrowserDrop = false;
            this.DaemonWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DaemonWebBrowser.IsWebBrowserContextMenuEnabled = false;
            this.DaemonWebBrowser.Location = new System.Drawing.Point(0, 0);
            this.DaemonWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.DaemonWebBrowser.Name = "DaemonWebBrowser";
            this.DaemonWebBrowser.ScriptErrorsSuppressed = true;
            this.DaemonWebBrowser.Size = new System.Drawing.Size(624, 442);
            this.DaemonWebBrowser.TabIndex = 0;
            this.DaemonWebBrowser.WebBrowserShortcutsEnabled = false;
            this.DaemonWebBrowser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.DaemonWebBrowser_Navigating);
            this.DaemonWebBrowser.NewWindow += new System.ComponentModel.CancelEventHandler(this.DaemonWebBrowser_NewWindow);
            this.DaemonWebBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.DaemonWebBrowser_DocumentCompleted);
            // 
            // DaemonBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 442);
            this.Controls.Add(this.DaemonWebBrowser);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DaemonBrowser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Playdar daemon";
            this.Load += new System.EventHandler(this.DaemonBrowser_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DaemonBrowser_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser DaemonWebBrowser;
    }
}