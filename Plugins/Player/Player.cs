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
using Windar.PlayerPlugin.Commands;

namespace Windar.PlayerPlugin
{
    class Player
    {
        public delegate void PauseHandler(object sender, EventArgs e);
        public delegate void StopHandler(object sender, EventArgs e);

        public event PauseHandler PausePlayer;
        public event StopHandler StopPlayer;

        public bool Playing { get; private set; }

        public void Play(string filename)
        {
            var cmd = new Play {Filename = filename, Player = this};
            cmd.RunAsync();
            Playing = true;
        }

        public void Pause()
        {
            PausePlayer(this, new EventArgs());
        }

        public void Stop()
        {
            StopPlayer(this, new EventArgs());
            Playing = false;
        }
    }
}
