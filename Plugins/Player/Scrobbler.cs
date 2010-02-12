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
            var str = new StringBuilder();
            str.Append(_page.Plugin.Host.Paths.LocalPlaydarURL);
            str.Append("audioscrobbler/");
            return str.ToString();
        }

        public void Start(string artist, string album, string track, string source, int length)
        {
            var str = new StringBuilder();
            str.Append(BaseUrl());
            str.Append("start");
            str.Append("?a=").Append(artist);
            str.Append("&b=").Append(album);
            str.Append("&t=").Append(track);
            str.Append("&o=").Append(source);
            str.Append("&l=").Append(length);
            var url = str.ToString();
            if (Log.IsDebugEnabled) Log.Debug(string.Format("Scrobble start: {0}", url));
            var response = PlayerPlugin.WGet(url);
            if (!string.IsNullOrEmpty(response) && Log.IsDebugEnabled) Log.Debug("\n" + response);
            _state = State.Initial;
        }

        public void Resume()
        {
            var str = new StringBuilder();
            str.Append(BaseUrl());
            str.Append("resume");
            var url = str.ToString();
            if (Log.IsDebugEnabled) Log.Debug(string.Format("Scrobble resume: {0}", url));
            var response = PlayerPlugin.WGet(url);
            if (!string.IsNullOrEmpty(response) && Log.IsDebugEnabled) Log.Debug("\n" + response);
            _state = State.Resumed;
        }

        public void Pause()
        {
            var str = new StringBuilder();
            str.Append(BaseUrl());
            str.Append("pause");
            var url = str.ToString();
            if (Log.IsDebugEnabled) Log.Debug(string.Format("Scrobble pause: {0}", url));
            var response = PlayerPlugin.WGet(url);
            if (!string.IsNullOrEmpty(response) && Log.IsDebugEnabled) Log.Debug("\n" + response);
            _state = State.Paused;
        }

        public void Stop()
        {
            if (_state == State.Stopped) return;
            var str = new StringBuilder();
            str.Append(BaseUrl());
            str.Append("stop");
            var url = str.ToString();
            if (Log.IsDebugEnabled) Log.Debug(string.Format("Scrobble stop: {0}", url));
            var response = PlayerPlugin.WGet(url);
            if (!string.IsNullOrEmpty(response) && Log.IsDebugEnabled) Log.Debug("\n" + response);
            _state = State.Stopped;
        }
    }
}
