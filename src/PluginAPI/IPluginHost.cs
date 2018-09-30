using System.Windows.Forms;
using Windar.Common;

namespace Windar.PluginAPI
{
    public interface IPluginHost
    {
        WindarPaths Paths { get; }
        Credentials ScrobblerCredentials { get; set; }
        void AddTabPage(UserControl control, string title);
        void AddConfigurationPage(ConfigTabContent control, string title);
        void ApplyChangesRequiresDaemonRestart();
    }
}
