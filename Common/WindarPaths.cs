/*
 * Windar: Playdar for Windows
 * Copyright (C) 2009 Steven Robertson <steve@playnode.org>
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

using System;
using System.Text;

namespace Windar.Common
{
    public class WindarPaths
    {
        public string ApplicationPath { get; private set; }

        private string _erlPath;
        private string _erlCmd;
        private string _playdarPath;
        private string _playdarDataPath;

        public WindarPaths(string appPath)
        {
            ApplicationPath = appPath;
        }

        public string ErlPath
        {
            get
            {
                return _erlPath ?? (_erlPath = ApplicationPath + @"\minimerl");
            }
        }

        public string ErlCmd
        {
            get
            {
                return _erlCmd ?? (_erlCmd = ErlPath + @"\bin\erl.exe");
            }
        }

        public string PlaydarPath
        {
            get
            {
                return _playdarPath ?? (_playdarPath = ApplicationPath + @"\playdar");
            }
        }

        public string PlaydarDataPath
        {
            get
            {
                if (_playdarDataPath == null)
                {
                    var str = new StringBuilder();
                    str.Append(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                    str.Append(@"\Windar2");
                    _playdarDataPath = str.ToString();
                }
                return _playdarDataPath;
            }
        }

        public static string ToWindowsPath(string path)
        {
            return path.Replace('/', '\\');
        }

        public static string ToUnixPath(string path)
        {
            return path.Replace('\\', '/');
        }
    }
}
