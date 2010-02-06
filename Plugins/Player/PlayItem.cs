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

namespace Windar.PlayerPlugin
{
    class PlayItem
    {
        public string SId { get; set; }
        public string Artist { get; set; }
        public string Track { get; set; }
        public string Album { get; set; }
        public string MimeType { get; set; }
        public float Score { get; set; }
        public int Duration { get; set; }
        public int Bitrate { get; set; }
        public int Size { get; set; }
        public string Source { get; set; }
    }
}
