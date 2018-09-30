namespace Windar.PluginAPI
{
    public interface IPlugin
    {
        IPluginHost Host { get; set; }
        string Name { get; }
        void Load();
        void Shutdown();
    }
}
