using Windar.PluginAPI;

namespace Windar.SpotifyPlugin
{
    public class SpotifyPlugin : IPlugin
    {
        IPluginHost _host;

        public IPluginHost Host
        {
            get { return _host; }
            set { _host = value; }
        }

        public string Name
        {
            get
            {
                return "Spotify";
            }
        }

        public void Load()
        {
            Host.AddConfigurationPage(new ConfigTabContent(new SpotifyConfigForm(this)), Name);
        }

        public void Shutdown()
        {
        }
    }
}
