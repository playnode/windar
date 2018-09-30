using Windar.PluginAPI;

namespace Windar.NapsterPlugin
{
    public class NapsterPlugin : IPlugin
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
                return "Napster";
            }
        }

        public void Load()
        {
            Host.AddConfigurationPage(new ConfigTabContent(new NapsterConfigForm(this)), Name);
        }

        public void Shutdown()
        {
        }
    }
}
