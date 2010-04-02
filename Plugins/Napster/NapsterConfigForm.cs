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

using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Windar.Common;
using Windar.PluginAPI;

namespace Windar.NapsterPlugin
{
    public partial class NapsterConfigForm : UserControl, IConfigForm
    {
        public IConfigFormContainer FormContainer { get; set; }

        readonly NapsterPlugin _plugin;
        readonly SimplePropertiesFile _conf;
        readonly string _deviceid;
        string _origUsername;
        string _origPassword;

        public NapsterConfigForm(NapsterPlugin plugin)
        {
            InitializeComponent();
            _plugin = plugin;

            // Set the original values.
            var filename = _plugin.Host.Paths.PlaydarEtcPath + @"\napster.conf";
            _conf = new SimplePropertiesFile(filename);
            if (File.Exists(filename))
            {
                _origUsername = _conf.Sections["napster"]["username"];
                _origPassword = _conf.Sections["napster"]["password"];
                _deviceid = !_conf.Sections["napster"].ContainsKey("deviceid")
                    ? "" : _conf.Sections["napster"]["deviceid"];
            }
            else
            {
                _origUsername = "";
                _origPassword = "";
                _deviceid = "";
            }
        }

        void NapsterConfigForm_Load(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(_origUsername)) usernameTextbox.Text = _origUsername;
            if (!string.IsNullOrEmpty(_origPassword)) passwordTextbox.Text = _origPassword;
        }

        public void Save()
        {
            if (!_conf.Sections.ContainsKey("napster") || _conf.Sections["napster"] == null)
                _conf.Sections.Add("napster", new Dictionary<string, string>());
            _conf.Sections["napster"]["username"] = usernameTextbox.Text;
            _conf.Sections["napster"]["password"] = passwordTextbox.Text;
            _conf.Sections["napster"]["deviceid"] = _deviceid;
            _conf.Save();

            // Reset the original values to the new saved values.
            _origUsername = usernameTextbox.Text;
            _origPassword = passwordTextbox.Text;

            FormContainer.Changed = false;

            _plugin.Host.ApplyChangesRequiresDaemonRestart();
        }

        public void Cancel()
        {
            usernameTextbox.Text = !string.IsNullOrEmpty(_origUsername) ? _origUsername : "";
            passwordTextbox.Text = !string.IsNullOrEmpty(_origPassword) ? _origPassword : "";
        }

        void username_TextChanged(object sender, System.EventArgs e)
        {
            if (FormContainer != null)
                FormContainer.Changed = !usernameTextbox.Text.Equals(_origUsername);
        }

        void password_TextChanged(object sender, System.EventArgs e)
        {
            if (FormContainer != null)
                FormContainer.Changed = !passwordTextbox.Text.Equals(_origPassword);
        }

        void napsterLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.napster.com/");
        }

        bool Completed()
        {
            return usernameTextbox.Text != "" && passwordTextbox.Text != "";
        }

        bool Changed()
        {
            return !usernameTextbox.Text.Equals(_origUsername)
                   || !passwordTextbox.Text.Equals(_origPassword);
        }

        private void usernameTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && Completed() && Changed()) Save();
        }

        private void passwordTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && Completed() && Changed()) Save();
        }

        private void usernameTextbox_Enter(object sender, System.EventArgs e)
        {
            usernameTextbox.SelectAll();
        }

        private void passwordTextbox_Enter(object sender, System.EventArgs e)
        {
            passwordTextbox.SelectAll();
        }
    }
}
