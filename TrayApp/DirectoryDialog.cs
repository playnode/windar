/*************************************************************************
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * Copyright (C) 2009, 2010 Steven Robertson <steve@playnode.org>
 *
 * Windar: Playdar for Windows
 *
 * Windar is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 2 of the License, or
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
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Windar.TrayApp
{
    /// <summary>
    /// Using the Win32 API to find folders. Initially being used to find
    /// files or folders, but just folders now.
    /// More 
    /// </summary>
    /// <remarks>
    /// Good example of API constants:
    /// 
    /// </remarks>
    class DirectoryDialog
    {
        #region Win32 API

        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPStr)] string lParam);

        [DllImport("ole32", EntryPoint = "CoTaskMemFree", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        static extern int CoTaskMemFree(IntPtr hMem);

        [DllImport("shell32", EntryPoint = "SHBrowseForFolder", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        static extern IntPtr ShBrowseForFolder(ref BrowseInfo lpbi);

        [DllImport("shell32", EntryPoint = "SHGetPathFromIDList", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        static extern int ShGetPathFromIdList(IntPtr pidList, StringBuilder lpBuffer);

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

        #region BrwoseForTypes

        // Get more API constants from the following page:
        // http://www.pinvoke.net/default.aspx/shell32/ShBrowseForFolder.html
        const int BrowseDirs = 0x0001;
        const int BrowseComputers = 0x1000;
        const int BrowseFiles = 0x4000;
        const int BrowseNotBelowDomain = 0x0002;
        const int BrowseNoNewFolderButton = 0x0200;
        const int BrowseNoTranslate = 0x0400;

        public enum BrowseForTypes
        {
            Computers = BrowseComputers,
            Directories = BrowseDirs | BrowseNotBelowDomain | BrowseNoNewFolderButton | BrowseNoTranslate,
            FilesAndDirectories = Directories | BrowseFiles,
        }

        #endregion

        const int MaxPath = 260;

        #region Select folder callback

        public delegate int BrowseCallBackProc(IntPtr hwnd, int msg, IntPtr lp, IntPtr wp);

        public int OnBrowseEvent(IntPtr hWnd, int msg, IntPtr lp, IntPtr lpData)
        {
            if (msg == 1)
            {
                // BFFM_INITIALIZED = 1
                // BFFM_SETSELECTIONW = 0x400 + 103;
                if (!string.IsNullOrEmpty(InitialPath))
                    SendMessage(hWnd, 1127, new IntPtr(1), InitialPath);
            }
            return 0;
        }

        #endregion

        #region Properties

        public BrowseForTypes BrowseFor { get; set; }
        public string InitialPath { get; set; }
        public string Title { get; set; }
        public string Selected { get; set; }

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
