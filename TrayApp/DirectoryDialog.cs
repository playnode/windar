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
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Windar.TrayApp
{
    class DirectoryDialog
    {
        #region Win32 API

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, string lParam);

        [DllImport("ole32", EntryPoint = "CoTaskMemFree", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int CoTaskMemFree(IntPtr hMem);

        [DllImport("kernel32", EntryPoint = "lstrcat", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr Lstrcat(string lpString1, string lpString2);

        [DllImport("shell32", EntryPoint = "SHBrowseForFolder", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr ShBrowseForFolder(ref BrowseInfo lpbi);

        [DllImport("shell32", EntryPoint = "SHGetPathFromIDList", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int ShGetPathFromIdList(IntPtr pidList, StringBuilder lpBuffer);

        #endregion

        public struct BrowseInfo
        {
            public IntPtr WndOwner;
            public int IDLRoot;
            public string DisplayName;
            public string Title;
            public int Flags;
            public BrowseCallBackProc Callback;
            public int Param;
            public int Image;
        }

        public enum BrowseForTypes
        {
            Computers = 4096,
            Directories = 1539, //1,
            FilesAndDirectories = 16384,
            FileSystemAncestors = 8,
        }

        //private uint BIF_RETURNONLYFSDIRS = 0x0001;
        //private uint BIF_DONTGOBELOWDOMAIN = 0x0002;
        //private uint BIF_NONEWFOLDERBUTTON = 0x0200;
        //private uint BIF_NOTRANSLATETARGETS = 0x0400;

        //private uint BIF_BROWSEINCLUDEFILES = 0x4000;
        //private uint BIF_USENEWUI = 0x0040 + 0x0010;
        //private uint BIF_NEWDIALOGSTYLE = 0x0040;
        //private uint BIF_SHAREABLE = 0x8000;
        //private uint BIF_BROWSEFORCOMPUTER = 0x1000;

        const int MaxPath = 260;

        #region Select folder callback

        public delegate int BrowseCallBackProc(IntPtr hwnd, int msg, IntPtr lp, IntPtr wp);

        public int OnBrowseEvent(IntPtr hWnd, int msg, IntPtr lp, IntPtr lpData)
        {
            if (msg == 1) SendMessage(new HandleRef(null, hWnd), 1127, 1, InitialPath);
            return 0;
        }

        #endregion

        #region Properties

        public BrowseForTypes BrowseFor { get; set; }
        public string InitialPath { get; set; }
        public string Title { get; set; }
        public string Selected { get; private set; }

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
            if (BrowseFor == 0) BrowseFor = BrowseForTypes.FilesAndDirectories;
            var bi = new BrowseInfo();
            var hTitle = GCHandle.Alloc(Title, GCHandleType.Pinned);
            bi.WndOwner = hWndOwner;
            bi.Title = Title;
            bi.Flags = (int) BrowseFor;
            bi.Callback = new BrowseCallBackProc(OnBrowseEvent);
            var buffer = new StringBuilder(MaxPath) {Length = MaxPath};
            bi.DisplayName = buffer.ToString();
            var ptr = ShBrowseForFolder(ref bi);
            hTitle.Free();
            if (ptr.ToInt64() == 0) return false;
            if (BrowseFor == BrowseForTypes.Computers)
            {
                Selected = bi.DisplayName.Trim();
            }
            else
            {
                var path = new StringBuilder(MaxPath);
                ShGetPathFromIdList(ptr, path);
                Selected = path.ToString();
            }
            CoTaskMemFree(ptr);
            return true;
        }
    }
}
