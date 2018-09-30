using Windar.PluginAPI;

namespace Windar.ScrobblerPlugin
{
    public class ScrobblerPlugin : IPlugin
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
                return "Scrobbler";
            }
        }

        public void Load()
        {
            Host.AddConfigurationPage(new ConfigTabContent(new ScrobblerConfigForm(this)), Name);
        }

        public void Shutdown()
        {
        }
    }
}
