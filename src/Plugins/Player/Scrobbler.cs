using System.Reflection;
using System.Text;
using log4net;

namespace Windar.PlayerPlugin
{
    class Scrobbler
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        enum State
        {
            Initial,
            Resumed,
            Paused,
            Stopped
        }

        readonly PlayerTabPage _page;
        State _state;

        public Scrobbler(PlayerTabPage page)
        {
            _page = page;
            _state = State.Initial;
        }

        string BaseUrl()
        {
            StringBuilder str = new StringBuilder();
            str.Append(_page.Plugin.Host.Paths.LocalPlaydarUrl);
            str.Append("audioscrobbler/");
            return str.ToString();
        }

        public void Start(string artist, string album, string track, string source, int length)
        {
            StringBuilder str = new StringBuilder();
            str.Append(BaseUrl());
            str.Append("start");
            str.Append("?a=").Append(artist);
            str.Append("&b=").Append(album);
            str.Append("&t=").Append(track);
            str.Append("&o=").Append(source);
            str.Append("&l=").Append(length);
            string url = str.ToString();
            if (Log.IsDebugEnabled) Log.Debug(string.Format("Scrobble start: {0}", url));
            string response = PlayerPlugin.WGet(url);
            if (!string.IsNullOrEmpty(response) && Log.IsDebugEnabled) Log.Debug("\n" + response);
            _state = State.Initial;
        }

        public void Resume()
        {
            StringBuilder str = new StringBuilder();
            str.Append(BaseUrl());
            str.Append("resume");
            string url = str.ToString();
            if (Log.IsDebugEnabled) Log.Debug(string.Format("Scrobble resume: {0}", url));
            string response = PlayerPlugin.WGet(url);
            if (!string.IsNullOrEmpty(response) && Log.IsDebugEnabled) Log.Debug("\n" + response);
            _state = State.Resumed;
        }

        public void Pause()
        {
            StringBuilder str = new StringBuilder();
            str.Append(BaseUrl());
            str.Append("pause");
            string url = str.ToString();
            if (Log.IsDebugEnabled) Log.Debug(string.Format("Scrobble pause: {0}", url));
            string response = PlayerPlugin.WGet(url);
            if (!string.IsNullOrEmpty(response) && Log.IsDebugEnabled) Log.Debug("\n" + response);
            _state = State.Paused;
        }

        public void Stop()
        {
            if (_state == State.Stopped) return;
            StringBuilder str = new StringBuilder();
            str.Append(BaseUrl());
            str.Append("stop");
            string url = str.ToString();
            if (Log.IsDebugEnabled) Log.Debug(string.Format("Scrobble stop: {0}", url));
            string response = PlayerPlugin.WGet(url);
            if (!string.IsNullOrEmpty(response) && Log.IsDebugEnabled) Log.Debug("\n" + response);
            _state = State.Stopped;
        }
    }
}
