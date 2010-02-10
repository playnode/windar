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
using System.Text;
using log4net;
using Windar.Common;

namespace Windar.PlayerPlugin.Commands
{
    class MPlayer : AsyncCmd<MPlayer>
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        enum State
        {
            Initial,
            Running,
            Caching,
            Playing,
            Paused,
            Stopped,
            Error,
            Ended
        }

        readonly string _filename;
        readonly PlayItem _item;
        readonly Player _player;

        State _state;

        public MPlayer()
        {
            
        }

        public MPlayer(PlayItem item, string filename, Player player)
        {
            _filename = filename;
            _item = item;
            _player = player;
            _state = State.Initial;
        }

        public override void RunAsync()
        {
            Runner.CommandCompleted += CommandCompleted;
            Runner.CommandOutput += CommandOutput;
            Runner.CommandError += CommandError;

            var cmd = new StringBuilder();
            cmd.Append('"').Append(_player.Page.Plugin.Host.ProgramFilesPath);
            cmd.Append(@"mplayer\").Append("mplayer.exe\" -slave -quiet ");
            cmd.Append(_filename);

            Runner.RunCommand(cmd.ToString());
            _state = State.Running;
        }

        public void Pause()
        {
            if (_state != State.Playing)
            {
                if (Log.IsWarnEnabled) 
                    Log.Warn("Invalid state change. Not playing.");
            }
            else
            {
                Runner.Process.StandardInput.WriteLine("\npause\n");
                Runner.Process.StandardInput.Flush();
                _state = State.Paused;
            }
            if (Log.IsDebugEnabled)
                Log.Debug("Player state = " + _state);
        }

        public void Resume()
        {
            if (_state != State.Paused)
            {
                if (Log.IsWarnEnabled) 
                    Log.Warn("Invalid state change. Not paused.");
            }
            else
            {
                Runner.Process.StandardInput.WriteLine("\npause\n");
                Runner.Process.StandardInput.Flush();
                _state = State.Playing;
            }
            if (Log.IsDebugEnabled)
                Log.Debug("Player state = " + _state);
        }

        public void Stop()
        {
            if (_state == State.Stopped)
            {
                if (Log.IsWarnEnabled) Log.Warn("Invalid state change. Already stopped.");
            }
            else
            {
                Runner.Process.StandardInput.WriteLine("\nstop\n");
                Runner.Process.StandardInput.Flush();
                _state = State.Stopped;
            }
            if (Log.IsDebugEnabled)
                Log.Debug("Player state = " + _state);
        }

        protected void CommandCompleted(object sender, EventArgs e)
        {
            _state = State.Ended;
        }

        protected void CommandOutput(object sender, CmdRunner.CommandEventArgs e)
        {
            if (e.Text.Length > 11 && e.Text.Substring(0, 11).Equals("Cache fill:"))
            {
                _state = State.Caching;
                _player.Page.SetStatus(e.Text);
            }
            else if (e.Text.Length == 20 && e.Text.Equals("Starting playback..."))
            {
                _state = State.Playing;

                // Update the status.
                var str = new StringBuilder();
                str.Append("Playing: ");
                str.Append(_item.Artist).Append(" - ");
                str.Append(_item.Track).Append(" - ");
                str.Append(_item.Album).Append(" - ");
                str.Append(_item.Source);
                _player.Page.SetStatus(str.ToString());
                _player.Page.StateChanged();
            }
            else if (e.Text.Length == 24 && e.Text.Equals("Exiting... (End of file)"))
            {
                _state = State.Stopped;
                _player.Page.StopPlaying();
                _player.Page.StateChanged();
            }
        }

        protected void CommandError(object sender, CmdRunner.CommandEventArgs e)
        {
            // Ignore errors if player is stopped.
            if (_state == State.Stopped) return;

            _state = State.Error;
            if (e.Text.Length == 16 && e.Text.Equals("Failed, exiting."))
                _player.Page.SetStatus("Failed");
            else
                _player.Page.SetStatus("Error: " + e.Text);
        }
    }
}
