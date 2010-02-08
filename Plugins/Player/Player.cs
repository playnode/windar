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
using System.Reflection;
using log4net;
using Windar.PlayerPlugin.Commands;

namespace Windar.PlayerPlugin
{
    class Player
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        public delegate void PauseHandler(object sender, EventArgs e);
        public delegate void ResumeHandler(object sender, EventArgs e);
        public delegate void StopHandler(object sender, EventArgs e);

        public event PauseHandler PausePlayer;
        public event ResumeHandler ResumePlayer;
        public event StopHandler StopPlayer;

        public enum PlayState
        {
            Initial,
            Playing,
            Paused,
            Stopped
        }

        public PlayState State { get; private set; }

        private Play _cmd;

        internal PlayerTabPage Page { get; private set; }

        public Player(PlayerTabPage page)
        {
            Page = page;
            State = PlayState.Initial;
        }

        public void Play(string filename)
        {
            if (State == PlayState.Initial || State == PlayState.Stopped)
            {
                _cmd = new Play { Filename = filename, Player = this };
                _cmd.RunAsync();
            }
            State = PlayState.Playing;
            if (Log.IsDebugEnabled) Log.Debug("Player state = " + State);
        }

        public void Pause()
        {
            PausePlayer(this, new EventArgs());
            State = PlayState.Paused;
            if (Log.IsDebugEnabled) Log.Debug("Player state = " + State);
        }

        public void Resume()
        {
            ResumePlayer(this, new EventArgs());
            State = PlayState.Playing;
            if (Log.IsDebugEnabled) Log.Debug("Player state = " + State);
        }

        public void Stop()
        {
            StopPlayer(this, new EventArgs());
            State = PlayState.Stopped;
            if (Log.IsDebugEnabled) Log.Debug("Player state = " + State);

            // NOTE: The following command isn't working yet.
            if (_cmd != null) _cmd.ControlC();
        }
    }
}
