/*
 * Windar: Playdar for Windows
 * Copyright (C) 2009 Steven Robertson <http://stever.org.uk/>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System.Collections.Generic;
using com.echonest.api.v3.artist;

namespace Windar
{
    class Track
    {
        public string Artist { get; set; }
        public string Title { get; set; }

        public Track()
        {
            
        }

        public Track(string artist, string title)
        {
            Artist = artist;
            Title = title;
        }

        public List<Track> GetRecommendations()
        {
            var results = new List<Track>();

            // Get similar artists.
            var echonoestArtist = new ArtistAPI(Properties.Settings.Default.EchoNestAPIKey);
            var artists = echonoestArtist.searchArtist(Artist, true);
            for (var i = 0; i < artists.size(); i++)
            {
                var artist = (Artist) artists.get(i);
                var similars = echonoestArtist.getSimilarArtists(artist, 0, 10);
                for (var j = 0; j < similars.size(); j++)
                {
                    var simArtistScored = (Scored) similars.get(j);
                    var simArtist = (Artist) simArtistScored.getItem();
                    results.Add(new Track(simArtist.getName(), "none"));
                }
            }

            // TODO: Get tracks from similar artists.
            // TODO: Don't select any previously played recommendations.
            return results;
        }
    }
}
