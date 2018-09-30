using Windar.PluginAPI;

namespace Windar.MP3tunes
{
    public class MP3tunesPlugin : IPlugin
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
                return "MP3tunes";
            }
        }

        public void Load()
        {
            Host.AddConfigurationPage(new ConfigTabContent(new MP3tunesConfigForm(this)), Name);
        }

        public void Shutdown()
        {
        }
    }
}
