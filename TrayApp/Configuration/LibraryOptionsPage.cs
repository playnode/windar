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

using System.Collections.Generic;

namespace Windar.TrayApp.Configuration
{
    class LibraryOptionsPage : IOptionsPage
    {
        List<string> _scanPaths;
        bool _scanPathsChanged;

        public int SavedScanPathCount { get; set; }
        public bool ScanPathValueChanged { get; set; }
        public bool ScanPathsToAdd { get; set; }
        public bool ScanPathsRemoved { get; set; }

        public void Load()
        {
            SavedScanPathCount = ScanPaths.Count;
        }

        public bool Changed
        {
            get
            {
                return _scanPathsChanged
                       || ScanPathValueChanged
                       || ScanPathsToAdd;
            }
        }

        #region Page options

        #region Scan paths

        public List<string> ScanPaths
        {
            get
            {
                if (_scanPaths == null) 
                    _scanPaths = Program.Instance.Config.Main.ListScanPaths();
                return _scanPaths;
            }
        }

        public void RemoveScanPath(string path)
        {
            Program.Instance.Config.Main.RemoveScanPath(path);
            ScanPathsRemoved = true;
            _scanPaths = Program.Instance.Config.Main.ListScanPaths();
            _scanPathsChanged = true;
        }

        public void AddScanPath(string path)
        {
            Program.Instance.Config.Main.AddScanPath(path);
            _scanPaths = Program.Instance.Config.Main.ListScanPaths();
            _scanPathsChanged = true;
        }

        #endregion

        #endregion
    }
}
