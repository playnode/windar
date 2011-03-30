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

using System.Collections.Generic;

namespace Windar.TrayApp.Configuration
{
    class LibraryOptionsPage : IOptionsPage
    {
        List<string> _scanPaths;
        bool _scanPathsChanged;

        int _savedScanPathCount;
        bool _scanPathValueChanged;
        bool _scanPathsToAdd;
        bool _scanPathsRemoved;

        public int SavedScanPathCount
        {
            get { return _savedScanPathCount; }
            set { _savedScanPathCount = value; }
        }

        public bool ScanPathValueChanged
        {
            get { return _scanPathValueChanged; }
            set { _scanPathValueChanged = value; }
        }

        public bool ScanPathsToAdd
        {
            get { return _scanPathsToAdd; }
            set { _scanPathsToAdd = value; }
        }

        public bool ScanPathsRemoved
        {
            get { return _scanPathsRemoved; }
            set { _scanPathsRemoved = value; }
        }

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
                    _scanPaths = Program.Instance.Config.MainConfig.ListScanPaths();
                return _scanPaths;
            }
        }

        public void RemoveScanPath(string path)
        {
            Program.Instance.Config.MainConfig.RemoveScanPath(path);
            ScanPathsRemoved = true;
            _scanPaths = Program.Instance.Config.MainConfig.ListScanPaths();
            _scanPathsChanged = true;
        }

        public void AddScanPath(string path)
        {
            Program.Instance.Config.MainConfig.AddScanPath(path);
            _scanPaths = Program.Instance.Config.MainConfig.ListScanPaths();
            _scanPathsChanged = true;
        }

        #endregion

        #endregion
    }
}
