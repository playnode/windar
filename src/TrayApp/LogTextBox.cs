﻿/*
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * Copyright (C) 2009, 2010 Steven Robertson <steve@playnode.org>
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

using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using log4net;
using log4net.Appender;

namespace Windar.TrayApp
{
    public partial class LogTextBox : RichTextBox
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

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

        [DllImport("user32.dll")]
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

        const int SetRedraw = 0x000B;
        const int User = 0x400;
        const int GetEventMask = (User + 59);
        const int SetEventMask = (User + 69);

        // ReSharper restore UnaccessedField.Local
        // ReSharper restore UnusedMember.Local
        #pragma warning restore 649
        #pragma warning restore 169
        #endregion

        const string CommandPrefix = "CMD.INF: [erl.exe] ";
        const string ErrorCommandPrefix = "CMD.ERR: [erl.exe] ";
        const int MaxBufferSize = 1024 * 128; // 128K

        readonly MemoryAppender _memoryAppender;

        public bool Updating { get; set; }
        public bool FollowTail { get; set; }

        Timer _timer;
        string _buffer;
        bool _bufferChanged;
        int _linesRemoved;
        int _lineHeight;

        public LogTextBox()
        {
            InitializeComponent();
            SelectionProtected = true;
            FollowTail = true;

            // Set reference to memory appender.
            var appenders = LogManager.GetRepository().GetAppenders();
            foreach (var appender in appenders)
            {
                if (appender.Name != "MemoryAppender") continue;
                _memoryAppender = (MemoryAppender) appender;
                break;
            }

            // Register context menu event handlers.
            logBoxContextMenu.Items["copyMenuItem"].Click += CopyContextMenuItem_Click;
            logBoxContextMenu.Items["clearMenuItem"].Click += ClearContextMenuItem_Click;
        }

        int HorizontalScrollPosition
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

        public void Load()
        {
            // Set the log box font.
            const string preferredFontName = "ProFontWindows";
            var testFont = new Font(preferredFontName, 12, FontStyle.Regular, GraphicsUnit.Pixel);
            Font = testFont.Name == preferredFontName ? testFont :
                new Font("Courier New", 13, FontStyle.Regular, GraphicsUnit.Pixel);

            // Store line-height.
            _lineHeight = testFont.Name == preferredFontName ? 12 : 13;

            // Create timer for updating.
            _timer = new Timer { Interval = 250 };
            _timer.Tick += LogBoxTimer_Tick;

            // First update.
            _timer.Start();
            UpdateOutputBuffer();
            SetText(_buffer, _linesRemoved, _lineHeight);
        }

        void UpdateOutputBuffer()
        {
            var sb = new StringBuilder();
            try
            {
                var events = _memoryAppender.GetEvents();
                if (events.Length <= 0) return;
                foreach (var logEvent in events)
                {
                    var msg = logEvent.RenderedMessage;
                    if (msg.StartsWith(CommandPrefix))
                    {
                        var len = CommandPrefix.Length;
                        var str = msg.Substring(len, msg.Length - len).TrimEnd();
                        if (str.Length > 0)
                        {
                            sb.Append(str);
                            sb.Append('\n');
                        }
                    }
                    else if (msg.StartsWith(ErrorCommandPrefix))
                    {
                        var len = ErrorCommandPrefix.Length;
                        var str = msg.Substring(len, msg.Length - len).TrimEnd();
                        if (str.Length > 0)
                        {
                            sb.Append("ERROR! ").Append(str);
                            sb.Append('\n');
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (Log.IsErrorEnabled) Log.Error("Exception when reading log memory appender.", ex);
                _buffer = null;
            }

            // Clear appender until next update.
            if (_memoryAppender != null) _memoryAppender.Clear();

            // Update the log buffer.
            if (sb.Length > 0) sb.Remove(sb.Length - 1, 1); // Remove last newline.
            if (sb.Length > 0) _bufferChanged = true;
            var s = sb.ToString();
            sb = new StringBuilder();
            if (_buffer != null)
            {
                var trimmed = _buffer.TrimEnd();
                if (trimmed.Length > 0) sb.Append(trimmed).Append('\n');
            }
            sb.Append(s);

            // Remove lines from beginning of buffer if necessary.
            _linesRemoved = sb.Length <= MaxBufferSize ? _linesRemoved :
                _linesRemoved + TrimBuffer(sb, MaxBufferSize);

            _buffer = sb.ToString();
        }

        static int TrimBuffer(StringBuilder sb, int size)
        {
            // Trim from the beginning of the buffer to match size.
            var d1 = sb.ToString().Substring(0, sb.Length - size);
            sb.Remove(0, sb.Length - size);

            // Remove likely partial first line.
            var s = sb.ToString();
            var i = s.IndexOf('\n');
            var d2 = s.Substring(0, i); // Not including last newline.
            if (i > 0 && i < sb.Length) sb.Remove(0, i + 1);

            // How many lines removed?
            sb = new StringBuilder();
            sb.Append(d1);
            sb.Append(d2);
            s = sb.ToString();
            var n = s.Split('\n').Length;
            if (Log.IsDebugEnabled)
            {
                Log.Debug("Deleted string is:\n" + s);
                Log.Debug("Removed " + n + " lines from start of buffer.");
            }
            return n;
        }

        public void Close()
        {
            if (_timer != null) _timer.Stop();
        }

        public bool ScrollAtEnd
        {
            get
            {
                // Determine if scroll is at end position.
                // If so, automatically follow log tail.
                var si = new ScrollInfo();
                si.Size = (uint) Marshal.SizeOf(si);
                si.Mask = (uint) ScrollInfoMask.All;
                GetScrollInfo(Handle, (int) ScrollBarDirection.Vertical, ref si);
                var allow = si.Page / 2; // Allowing half-page difference.
                return si.Pos > si.Max - si.Page - allow;
            }
        }

        public void ScrollToEnd()
        {
            if (Log.IsDebugEnabled) Log.Debug("ScrollToEnd");

            //NOTE: Following didn't work well enough.
            //SelectionStart = TextLength;
            //ScrollToCaret();

            // Get the current scroll info.
            var si = new ScrollInfo();
            si.Size = (uint) Marshal.SizeOf(si);
            si.Mask = (uint) ScrollInfoMask.All;
            GetScrollInfo(Handle, (int) ScrollBarDirection.Vertical, ref si);

            // Set the scroll position to maximum.
            si.Pos = si.Max - (int) si.Page;
            SetScrollInfo(Handle, (int) ScrollBarDirection.Vertical, ref si, true);
            SendMessage(Handle, VerticalScroll, new IntPtr(Thumbtrack + 0x10000 * si.Pos), new IntPtr(0));

            FollowTail = true;
        }

        /// <summary>
        /// This method is just used to get the text to reposition as well as possible
        /// when the window is resized.
        /// </summary>
        public void ReSetText()
        {
            SetText(_buffer, _linesRemoved, _lineHeight);
        }

        void SetText(string text, int linesRemoved, int lineHeight)
        {
            var followTail = FollowTail;
            if (string.IsNullOrEmpty(text)) return;
            var pos = HorizontalScrollPosition;
            var eventMask = IntPtr.Zero;
            try
            {
                // Stop redrawing:
                SendMessage(Handle, SetRedraw, (IntPtr) 0, IntPtr.Zero);

                // Stop sending of events:
                eventMask = SendMessage(Handle, GetEventMask, (IntPtr) 0, IntPtr.Zero);

                // Replace the text.
                Clear();
                SelectedText = text;
                SelectionLength = SelectionStart = 0;

                // Update the scroll position.
                HorizontalScrollPosition = pos - (linesRemoved * lineHeight);
            }
            catch (Exception ex)
            {
                if (Log.IsErrorEnabled) Log.Error("Exception", ex);
                Text = _buffer = "";
            }
            finally
            {
                // Turn on events.
                SendMessage(Handle, SetEventMask, (IntPtr) 0, eventMask);

                // Turn on redrawing.
                SendMessage(Handle, SetRedraw, (IntPtr) 1, IntPtr.Zero);

                Invalidate();
                Application.DoEvents();
                FollowTail = followTail;
                if (FollowTail) ScrollToEnd();
            }
        }

        void RichTextBoxPlus_VScroll(object sender, EventArgs e)
        {
            FollowTail = ScrollAtEnd;
        }

        void LogBoxTimer_Tick(object sender, EventArgs e)
        {
            UpdateOutputBuffer();
            if (!Updating || !_bufferChanged) return;
            SetText(_buffer, _linesRemoved, _lineHeight);

            // Reset the following buffer-related vars.
            _bufferChanged = false;
            _linesRemoved = 0;
        }

        void CopyContextMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedText.Length > 0) Clipboard.SetText(SelectedText);
            else Clipboard.SetText(Text);
        }

        void ClearContextMenuItem_Click(object sender, EventArgs e)
        {
            _buffer = null;
            Clear();
        }
    }
}