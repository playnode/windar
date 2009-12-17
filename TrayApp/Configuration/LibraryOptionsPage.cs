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

using System.Collections.Generic;

namespace Windar.TrayApp.Configuration
{
    class LibraryOptionsPage : IOptionsPage
    {
        private bool _scanpathsChange;

        public bool ScanPathValueChanged { get; set; }
        public bool NewPathsToAdd { get; set; }

        public void Load()
        {
            // Nothing to do.
        }

        public bool Changed
        {
            get
            {
                return _scanpathsChange
                       || ScanPathValueChanged
                       || NewPathsToAdd;
            }
        }

        #region Page options

        #region Scan paths

        public List<string> ScanPaths
        {
            get
            {
                return Program.Instance.Config.Main.ListScanPaths();
            }
        }

        public void RemoveScanPath(string path)
        {
            Program.Instance.Config.Main.RemoveScanPath(path);
            _scanpathsChange = true;
        }

        public void AddScanPath(string path)
        {
            Program.Instance.Config.Main.AddScanPath(path);
            _scanpathsChange = true;
        }

        #endregion

        #endregion
    }
}
