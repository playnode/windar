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

namespace Windar.PluginAPI
{
    partial class ConfigTabContent
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
            this.configCancelButton = new System.Windows.Forms.Button();
            this.configSaveButton = new System.Windows.Forms.Button();
            this.propsPanel = new System.Windows.Forms.Panel();
            this.configContentPanel = new System.Windows.Forms.Panel();
            this.propsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // configCancelButton
            // 
            this.configCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.configCancelButton.Enabled = false;
            this.configCancelButton.Location = new System.Drawing.Point(75, 237);
            this.configCancelButton.Name = "configCancelButton";
            this.configCancelButton.Size = new System.Drawing.Size(60, 23);
            this.configCancelButton.TabIndex = 19;
            this.configCancelButton.Text = "Cancel";
            this.configCancelButton.UseVisualStyleBackColor = true;
            this.configCancelButton.Click += new System.EventHandler(this.configCancelButton_Click);
            // 
            // configSaveButton
            // 
            this.configSaveButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.configSaveButton.Enabled = false;
            this.configSaveButton.Location = new System.Drawing.Point(9, 237);
            this.configSaveButton.Name = "configSaveButton";
            this.configSaveButton.Size = new System.Drawing.Size(60, 23);
            this.configSaveButton.TabIndex = 18;
            this.configSaveButton.Text = "Save";
            this.configSaveButton.UseVisualStyleBackColor = true;
            this.configSaveButton.Click += new System.EventHandler(this.configSaveButton_Click);
            // 
            // propsPanel
            // 
            this.propsPanel.BackColor = System.Drawing.SystemColors.Control;
            this.propsPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.propsPanel.Controls.Add(this.configContentPanel);
            this.propsPanel.Controls.Add(this.configCancelButton);
            this.propsPanel.Controls.Add(this.configSaveButton);
            this.propsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propsPanel.Location = new System.Drawing.Point(0, 0);
            this.propsPanel.Name = "propsPanel";
            this.propsPanel.Padding = new System.Windows.Forms.Padding(6);
            this.propsPanel.Size = new System.Drawing.Size(504, 271);
            this.propsPanel.TabIndex = 1;
            // 
            // configContentPanel
            // 
            this.configContentPanel.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.configContentPanel.Location = new System.Drawing.Point(10, 10);
            this.configContentPanel.Name = "configContentPanel";
            this.configContentPanel.Size = new System.Drawing.Size(482, 217);
            this.configContentPanel.TabIndex = 20;
            // 
            // ConfigTabContent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.propsPanel);
            this.Name = "ConfigTabContent";
            this.Size = new System.Drawing.Size(504, 271);
            this.propsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        System.Windows.Forms.Button configCancelButton;
        System.Windows.Forms.Button configSaveButton;
        System.Windows.Forms.Panel propsPanel;
        System.Windows.Forms.Panel configContentPanel;
    }
}
