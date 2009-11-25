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
    partial class LogControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.logBox = new Windar.TrayApp.RichTextBoxPlus();
            this.LogContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LogContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // logBox
            // 
            this.logBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.logBox.ContextMenuStrip = this.LogContextMenu;
            this.logBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.logBox.DetectUrls = false;
            this.logBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logBox.Location = new System.Drawing.Point(0, 0);
            this.logBox.Name = "logBox";
            this.logBox.Position = 0;
            this.logBox.ReadOnly = true;
            this.logBox.ShortcutsEnabled = false;
            this.logBox.Size = new System.Drawing.Size(400, 300);
            this.logBox.TabIndex = 1;
            this.logBox.Text = "";
            this.logBox.WordWrap = false;
            // 
            // LogContextMenu
            // 
            this.LogContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyContextMenuItem,
            this.clearContextMenuItem});
            this.LogContextMenu.Name = "LogContextMenu";
            this.LogContextMenu.Size = new System.Drawing.Size(153, 70);
            // 
            // copyContextMenuItem
            // 
            this.copyContextMenuItem.Name = "copyContextMenuItem";
            this.copyContextMenuItem.Size = new System.Drawing.Size(152, 22);
            this.copyContextMenuItem.Text = "Copy";
            this.copyContextMenuItem.Click += new System.EventHandler(this.copyContextMenuItem_Click);
            // 
            // clearContextMenuItem
            // 
            this.clearContextMenuItem.Name = "clearContextMenuItem";
            this.clearContextMenuItem.Size = new System.Drawing.Size(152, 22);
            this.clearContextMenuItem.Text = "Clear";
            this.clearContextMenuItem.Click += new System.EventHandler(this.clearContextMenuItem_Click);
            // 
            // LogControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.logBox);
            this.Name = "LogControl";
            this.Size = new System.Drawing.Size(400, 300);
            this.Load += new System.EventHandler(this.Log_Load);
            this.LogContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private RichTextBoxPlus logBox;
        private System.Windows.Forms.ContextMenuStrip LogContextMenu;
        private System.Windows.Forms.ToolStripMenuItem copyContextMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearContextMenuItem;
    }
}
