/*
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * Copyright (C) 2009, 2010, 2011 Steven Robertson <steve@playnode.com>
 *
 * Windar - Playdar for Windows
 *
 * Windar is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License (LGPL) as published
 * by the Free Software Foundation; either version 2.1 of the License, or (at
 * your option) any later version.
 *
 * Windar is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License version 2.1 for more details
 * (a copy is included in the LICENSE file that accompanied this code).
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

namespace Windar.PlayerPlugin
{
    class PlayItem
    {
        string _sId;
        string _artist;
        string _track;
        string _album;
        string _mimeType;
        float _score;
        int _duration;
        int _bitrate;
        int _size;
        string _source;

        public string SId
        {
            get { return _sId; }
            set { _sId = value; }
        }

        public string Artist
        {
            get { return _artist; }
            set { _artist = value; }
        }

        public string Track
        {
            get { return _track; }
            set { _track = value; }
        }

        public string Album
        {
            get { return _album; }
            set { _album = value; }
        }

        public string MimeType
        {
            get { return _mimeType; }
            set { _mimeType = value; }
        }

        public float Score
        {
            get { return _score; }
            set { _score = value; }
        }

        public int Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        public int Bitrate
        {
            get { return _bitrate; }
            set { _bitrate = value; }
        }

        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }
    }
}
