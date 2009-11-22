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

using System;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Windar.TrayApp
{
    class DirectoryDialog
    {
        [DllImport("ole32", EntryPoint = "CoTaskMemFree", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int CoTaskMemFree(IntPtr hMem);

        [DllImport("kernel32", EntryPoint = "lstrcat", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr Lstrcat(string lpString1, string lpString2);

        [DllImport("shell32", EntryPoint = "SHBrowseForFolder", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr ShBrowseForFolder(ref BrowseInfo lpbi);

        [DllImport("shell32", EntryPoint = "SHGetPathFromIDList", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int ShGetPathFromIdList(IntPtr pidList, StringBuilder lpBuffer);

        public struct BrowseInfo
        {
            public IntPtr WndOwner;
            public int IDLRoot;
            public string DisplayName;
            public string Title;
            public int Flags;
            public int Callback;
            public int Param;
            public int Image;
        }

        public enum BrowseForTypes
        {
            Computers = 4096,
            Directories = 1,
            FilesAndDirectories = 16384,
            FileSystemAncestors = 8
        }

        const int MaxPath = 260;

        #region Properties

        private BrowseForTypes _browseFor = BrowseForTypes.Directories;
        private string _title = "";
        private string _selected = "";

        public BrowseForTypes BrowseFor
        {
            get { return _browseFor; }
            set { _browseFor = value; }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (ReferenceEquals(value, DBNull.Value))
                {
                    throw new ArgumentNullException();
                }
                _title = value;
            }
        }

        public string Selected
        {
            get { return _selected; }
        }

        #endregion

        public DialogResult ShowDialog()
        {
            return ShowDialog(null);
        }

        public DialogResult ShowDialog(IWin32Window owner)
        {
            var handle = owner != null ? owner.Handle : IntPtr.Zero;
            return RunDialog(handle) ? DialogResult.OK : DialogResult.Cancel;
        }

        protected bool RunDialog(IntPtr hWndOwner)
        {
            var bi = new BrowseInfo();
            var hTitle = GCHandle.Alloc(Title, GCHandleType.Pinned);
            bi.WndOwner = hWndOwner;
            bi.Title = Title;
            bi.Flags = (int) BrowseFor;
            var buffer = new StringBuilder(MaxPath) {Length = MaxPath};
            bi.DisplayName = buffer.ToString();
            var ptr = ShBrowseForFolder(ref bi);
            hTitle.Free();
            if (ptr.ToInt64() == 0) return false;
            if (BrowseFor == BrowseForTypes.Computers)
            {
                _selected = bi.DisplayName.Trim();
            }
            else
            {
                var path = new StringBuilder(MaxPath);
                ShGetPathFromIdList(ptr, path);
                _selected = path.ToString();
            }
            CoTaskMemFree(ptr);
            return true;
        }
    }
}
