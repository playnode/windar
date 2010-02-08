/*************************************************************************
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * Copyright (C) 2009, 2010 Steven Robertson <steve@playnode.org>
 *
 * Windar: Playdar for Windows
 *
 * Windar is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Windar is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 ************************************************************************/

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

        public IPluginHost Host { internal get; set; }

        readonly PlayerTabPage _tabPage;

        public string Name
        {
            get
            {
                return "Player";
            }
        }

        public PlayerPlugin()
        {
            _tabPage = new PlayerTabPage(this);
        }

        public void Load()
        {
            Host.AddTabPage(_tabPage, Name);
        }

        public void Shutdown()
        {
            //TODO: Stop mplayer if playing!
        }

        internal static string WGet(string url)
        {
            string result = null;

            if (Log.IsDebugEnabled) Log.Debug("WGet " + url);
            var request = (HttpWebRequest) WebRequest.Create(url);
            request.Timeout = 10000; // 10 secs
            request.UserAgent = "Windar";
            try
            {
                var response = (HttpWebResponse) request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var enc = Encoding.GetEncoding(1252);
                    var stream = new StreamReader(response.GetResponseStream(), enc);
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
    }
}
