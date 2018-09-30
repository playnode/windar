using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using log4net;
using Windar.PluginAPI;

namespace Windar.PlayerPlugin
{
    public class PlayerPlugin : IPlugin
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        readonly PlayerTabPage _tabPage;

        public PlayerPlugin()
        {
            _tabPage = new PlayerTabPage(this);
        }

        #region IPlugin implementation

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
                return "Player";
            }
        }

        public void Load()
        {
            Host.AddTabPage(_tabPage, Name);
        }

        public void Shutdown()
        {
            // Stop mplayer if playing!
            if (_tabPage.Player != null)
                _tabPage.Player.Stop();
        }

        #endregion

        internal static string WGet(string url)
        {
            string result = null;

            if (Log.IsDebugEnabled) Log.Debug("WGet " + url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 10000; // 10 secs
            request.UserAgent = "Windar";
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Encoding enc = Encoding.GetEncoding(1252);
                    StreamReader stream = new StreamReader(response.GetResponseStream(), enc);
                    result = stream.ReadToEnd();
                    response.Close();
                    stream.Close();
                    if (Log.IsDebugEnabled) Log.Debug("WGet result:\n" + result);
                }
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                {
                    if (Log.IsErrorEnabled) Log.Error("Exception", ex);
                }
                else
                {
                    switch (((HttpWebResponse) ex.Response).StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                            if (Log.IsErrorEnabled) Log.Error("404 Not Found");
                            break; // Ignore.
                        default:
                            if (Log.IsErrorEnabled) Log.Error("Exception", ex);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                if (Log.IsErrorEnabled) Log.Error("Exception", ex);
            }
            return result;
        }

        internal static string GetPlayingMessage(PlayItem item)
        {
            StringBuilder str = new StringBuilder();
            str.Append("Playing: ");
            str.Append(item.Artist).Append(" - ");
            str.Append(item.Track);
            if (!string.IsNullOrEmpty(item.Album))
                str.Append("   ///   ").Append(item.Album);
            str.Append("   ///   ").Append(item.Source);
            return str.ToString();
        }
    }
}
