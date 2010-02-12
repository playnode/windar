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
using System.Text;

namespace Windar.Common
{
    public class WindarPaths
    {
        public string WindarProgramFiles { get; set; }

        public WindarPaths(string appPath)
        {
            WindarProgramFiles = appPath;
        }

        public string LocalPlaydarURL
        {
            get
            {
                //TODO: Get the configured URL from config.
                return "http://127.0.0.1:60210/";
            }
        }

        string _erlPath;
        public string ErlPath
        {
            get
            {
                return _erlPath ?? 
                    (_erlPath = WindarProgramFiles + @"\minimerl");
            }
        }

        string _erlCmd;
        public string ErlCmd
        {
            get
            {
                return _erlCmd ?? 
                    (_erlCmd = ErlPath + @"\bin\erl.exe");
            }
        }

        string _playdarPath;
        public string PlaydarProgramPath
        {
            get
            {
                return _playdarPath ?? 
                    (_playdarPath = WindarProgramFiles + @"\playdar");
            }
        }

        string _playdarEtcPath;
        public string PlaydarEtcPath
        {
            get
            {
                return _playdarEtcPath ??
                    (_playdarEtcPath = WindarAppData + @"\etc");
            }
        }

        string _playdarDataPath;
        public string WindarAppData
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

        #region Conversion methods.

        public static string ToWindowsPath(string path)
        {
            return path.Replace('/', '\\');
        }

        public static string ToUnixPath(string path)
        {
            return path.Replace('\\', '/');
        }

        #endregion
    }
}
