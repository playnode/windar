namespace Windar.PluginAPI
{
    public interface IConfigForm
    {
        IConfigFormContainer FormContainer { set; }
        void Save();
        void Cancel();
    }
}
