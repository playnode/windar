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
using log4net;
using Windar.PlayerPlugin.Commands;

namespace Windar.PlayerPlugin
{
    /// <summary>
    /// This class uses a MPlayer command class for each track to be played.
    /// </summary>
    class Player
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        public enum State
        {
            Initial,
            Playing,
            Paused,
            Stopped
        }

        MPlayer _cmd;
        int _volume = 100;

        public PlayerTabPage Page { get; private set; }
        public State PlayerState { get; private set; }

        public int Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                _volume = value;
                if (_cmd != null) 
                    _cmd.ChangeVolume(_volume);
            }
        }

        public int Progress
        {
            get
            {
                _cmd.RequestProgress();
                return _cmd.Progress;
            }
        }

        public Player(PlayerTabPage page)
        {
            Page = page;
            PlayerState = State.Initial;
        }

        public void Play(PlayItem item, string filename)
        {
            if (PlayerState == State.Initial || PlayerState == State.Stopped)
            {
                _cmd = new MPlayer(item, filename, this) {Volume = _volume};
                _cmd.RunAsync();
                PlayerState = State.Playing;
            }
            else
            {
                if (Log.IsWarnEnabled) 
                    Log.Warn("Invalid state change. Not starting or stopped.");
            }
            if (Log.IsDebugEnabled) 
                Log.Debug("Player state = " + PlayerState);
        }

        public void Pause()
        {
            if (PlayerState != State.Playing)
            {
                if (Log.IsWarnEnabled) 
                    Log.Warn("Not playing.");
            }
            else
            {
                _cmd.Pause();
                PlayerState = State.Paused;
            }
            if (Log.IsDebugEnabled) 
                Log.Debug("Player state = " + PlayerState);
        }

        public void Resume()
        {
            if (PlayerState != State.Paused)
            {
                if (Log.IsWarnEnabled) 
                    Log.Warn("Invalid state change. Not paused.");
            }
            else
            {
                _cmd.Resume();
                PlayerState = State.Playing;
            }
            if (Log.IsDebugEnabled)
                Log.Debug("Player state = " + PlayerState);
        }

        public void Stop()
        {
            switch (PlayerState)
            {
                case State.Paused:
                case State.Playing:
                    _cmd.Stop();
                    _cmd = null;
                    PlayerState = State.Stopped;
                    break;
                case State.Stopped:
                    if (Log.IsWarnEnabled)
                        Log.Warn("Invalid state change. Already stopped.");
                    break;
                default:
                    if (Log.IsWarnEnabled) 
                        Log.Warn("Invalid state change. Not playing or paused.");
                    break;
            }
            if (Log.IsDebugEnabled)
                Log.Debug("Player state = " + PlayerState);
        }

        internal void Stopped()
        {
            PlayerState = State.Stopped;
        }
    }
}
