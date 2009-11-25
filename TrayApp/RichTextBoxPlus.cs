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
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using log4net;

namespace Windar.TrayApp
{
    public partial class RichTextBoxPlus : RichTextBox
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        #region Win32 API
        #pragma warning disable 649
        #pragma warning disable 169
        // ReSharper disable UnaccessedField.Local
        // ReSharper disable UnusedMember.Local

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetScrollInfo(IntPtr hWnd, int scrollDirection, ref ScrollInfo si);

        [DllImport("user32.dll")]
        static extern int SetScrollInfo(IntPtr hWnd, int scrollDirection, [In] ref ScrollInfo si, bool redraw);

        [DllImport("User32.dll", CharSet = CharSet.Auto, EntryPoint = "SendMessage")]
        static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        struct ScrollInfo
        {
            public uint Size;
            public uint Mask;
            public int Min;
            public int Max;
            public uint Page;
            public int Pos;
            public int TrackPos;
        }

        enum ScrollInfoMask
        {
            Range = 0x1,
            Page = 0x2,
            Pos = 0x4,
            DisableEndScroll = 0x8,
            TrackPos = 0x10,
            All = Range + Page + Pos + TrackPos
        }

        enum ScrollBarDirection
        {
            Horizontal = 0,
            Vertical = 1,
            Ctl = 2,
            Both = 3
        }

        const int VerticalScroll = 277;
        const int LineUp = 0;
        const int LineDown = 1;
        const int ThumbPosition = 4;
        const int Thumbtrack = 5;
        const int ScrollTop = 6;
        const int ScrolBottom = 7;
        const int EndScroll = 8;

        // ReSharper restore UnaccessedField.Local
        // ReSharper restore UnusedMember.Local
        #pragma warning restore 649
        #pragma warning restore 169
        #endregion

        private bool _ignoreVScroll;

        #region Properties

        public bool FollowTail { get; private set; }

        public bool ScrollAtEnd
        {
            get
            {
                var si = new ScrollInfo();
                si.Size = (uint) Marshal.SizeOf(si);
                si.Mask = (uint) ScrollInfoMask.All;
                GetScrollInfo(Handle, (int) ScrollBarDirection.Vertical, ref si);
                var allow = si.Page / 2; // Allowing half-page difference.
                return si.Pos > si.Max - si.Page - allow;
            }
        }

        public int Position
        {
            get
            {
                var si = new ScrollInfo();
                si.Size = (uint) Marshal.SizeOf(si);
                si.Mask = (uint) ScrollInfoMask.All;
                GetScrollInfo(Handle, (int) ScrollBarDirection.Vertical, ref si);
                return si.Pos;
            }
            set
            {
                var si = new ScrollInfo();
                si.Size = (uint) Marshal.SizeOf(si);
                si.Mask = (uint) ScrollInfoMask.All;
                GetScrollInfo(Handle, (int) ScrollBarDirection.Vertical, ref si);
                si.Pos = value;
                SetScrollInfo(Handle, (int) ScrollBarDirection.Vertical, ref si, true);
                SendMessage(Handle, VerticalScroll, new IntPtr(Thumbtrack + 0x10000 * si.Pos), new IntPtr(0));
            }
        }

        #endregion

        public RichTextBoxPlus()
        {
            InitializeComponent();
            SelectionProtected = true;
            FollowTail = true;
        }

        public void ScrollToEnd()
        {
            // Get the current scroll info.
            var si = new ScrollInfo();
            si.Size = (uint) Marshal.SizeOf(si);
            si.Mask = (uint) ScrollInfoMask.All;
            GetScrollInfo(Handle, (int) ScrollBarDirection.Vertical, ref si);

            // Set the scroll position to maximum.
            si.Pos = si.Max - (int) si.Page;
            SetScrollInfo(Handle, (int) ScrollBarDirection.Vertical, ref si, true);
            SendMessage(Handle, VerticalScroll, new IntPtr(Thumbtrack + 0x10000 * si.Pos), new IntPtr(0));
        }

        public void SetText(string text, int linesRemoved, int lineHeight)
        {
            if (string.IsNullOrEmpty(text)) return;
            _ignoreVScroll = true;
            var pos = Position;
            
            SuspendLayout();
            
            Text = text;
            Position = pos - (linesRemoved * lineHeight);
            _ignoreVScroll = false;
            if (FollowTail) ScrollToEnd();

            ResumeLayout();
            Update();
            Application.DoEvents();
        }

        private void RichTextBoxPlus_VScroll(object sender, EventArgs e)
        {
            if (_ignoreVScroll) return;
            if (Log.IsDebugEnabled) Log.Debug("RichTextBoxPlus_VScroll");
            FollowTail = ScrollAtEnd;
        }
    }
}
