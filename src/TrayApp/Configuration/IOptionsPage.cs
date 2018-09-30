namespace Windar.TrayApp.Configuration
{
    interface IOptionsPage
    {
        bool Changed { get; }
        void Load();
    }
}
