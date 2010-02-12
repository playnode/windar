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

using System.IO;
using System.Text;
using System.Windows.Forms;
using Windar.PluginAPI;

namespace Windar.NapsterPlugin
{
    public partial class NapsterConfigForm : UserControl, IConfigForm
    {
        public IConfigFormContainer FormContainer { private get; set; }

        readonly NapsterPlugin _plugin;

        string _origUsername;
        string _origPassword;

        readonly string _deviceId;

        #region Configuration filename.

        string _configFilename;
        string ConfigFilename
        {
            get
            {
                return _configFilename ??
                    (_configFilename = _plugin.Host.Paths.PlaydarEtcPath + @"\napster.conf");
            }
        }

        #endregion

        public NapsterConfigForm(NapsterPlugin plugin)
        {
            InitializeComponent();
            _plugin = plugin;
            var newFile = false;

            // Check if the configuration file already exists.
            if (File.Exists(ConfigFilename))
            {
                // Load config from file.
                var streamReader = new StreamReader(ConfigFilename);
                var line1 = streamReader.ReadLine();
                if (!line1.Equals("[napster]")) newFile = true;
                else
                {
                    var line2 = streamReader.ReadLine();
                    if (!line2.StartsWith("username = ")) newFile = true;
                    else
                    {
                        var line3 = streamReader.ReadLine();
                        if (!line3.StartsWith("password = ")) newFile = true;
                        else
                        {
                            _origUsername = line2.Substring(11, line2.Length - 11);
                            _origPassword = line3.Substring(11, line3.Length - 11);
                        }

                        var line4 = streamReader.ReadLine();
                        if (line4.StartsWith("deviceid = "))
                            _deviceId = line4.Substring(11, line4.Length - 11);
                    }
                }
                streamReader.Close();
            }
            if (!newFile) return;
            _origPassword = "";
            _origUsername = "";
        }

        private void NapsterConfigForm_Load(object sender, System.EventArgs e)
        {
            usernameTextbox.Text = _origUsername;
            passwordTextbox.Text = _origPassword;
        }

        public void Save()
        {
            var str = new StringBuilder();
            str.Append("[napster]\n");
            str.Append("username = ").Append(usernameTextbox.Text).Append("\n");
            str.Append("password = ").Append(passwordTextbox.Text).Append("\n");
            str.Append("deviceid = ").Append(_deviceId).Append("\n");

            // Write the configuration file.
            if (File.Exists(ConfigFilename)) File.Delete(ConfigFilename);
            var file = new FileStream(ConfigFilename, FileMode.Create, FileAccess.Write);
            var sw = new StreamWriter(file);
            sw.Write(str.ToString());
            sw.Close();
            file.Close();

            _origUsername = usernameTextbox.Text;
            _origPassword = passwordTextbox.Text;

            FormContainer.Changed = false;
        }

        public void Cancel()
        {
            usernameTextbox.Text = _origUsername;
            passwordTextbox.Text = _origPassword;
        }

        private void username_TextChanged(object sender, System.EventArgs e)
        {
            if (FormContainer != null)
                FormContainer.Changed = !usernameTextbox.Text.Equals(_origUsername);
        }

        private void password_TextChanged(object sender, System.EventArgs e)
        {
            if (FormContainer != null)
                FormContainer.Changed = !passwordTextbox.Text.Equals(_origPassword);
        }

        private void napsterLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.napster.com/");
        }
    }
}
