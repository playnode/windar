namespace Windar.Common
{
    public abstract class AsyncCmd<T> : Cmd<T> where T : new()
    {
        public abstract void RunAsync();
    }
}
