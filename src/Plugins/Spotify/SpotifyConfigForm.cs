using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Windar.Common;
using Windar.PluginAPI;

namespace Windar.SpotifyPlugin
{
    public partial class SpotifyConfigForm : UserControl, IConfigForm
    {
        IConfigFormContainer _formContainer;

        public IConfigFormContainer FormContainer
        {
            get { return _formContainer; }
            set { _formContainer = value; }
        }

        readonly SpotifyPlugin _plugin;
        readonly SimplePropertiesFile _conf;
        string _origUsername;
        string _origPassword;

        public SpotifyConfigForm(SpotifyPlugin plugin)
        {
            InitializeComponent();
            _plugin = plugin;

            // Set the original values.
            string filename = _plugin.Host.Paths.PlaydarEtcPath + @"\spotify.conf";
            _conf = new SimplePropertiesFile(filename);
            if (File.Exists(filename))
            {
                _origUsername = _conf.Sections["spotify"]["username"];
                _origPassword = _conf.Sections["spotify"]["password"];
            }
            else
            {
                _origUsername = "";
                _origPassword = "";
            }
        }

        void SpotifyConfigForm_Load(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(_origUsername)) usernameTextbox.Text = _origUsername;
            if (!string.IsNullOrEmpty(_origPassword)) passwordTextbox.Text = _origPassword;
        }

        public void Save()
        {
            if (!_conf.Sections.ContainsKey("spotify") || _conf.Sections["spotify"] == null)
                _conf.Sections.Add("spotify", new Dictionary<string, string>());
            _conf.Sections["spotify"]["username"] = usernameTextbox.Text;
            _conf.Sections["spotify"]["password"] = passwordTextbox.Text;
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

        void spotifyLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.spotify.com/");
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
