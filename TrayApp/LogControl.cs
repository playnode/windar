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
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using log4net;
using log4net.Appender;

namespace Windar.TrayApp
{
    partial class LogControl : UserControl
    {
        //TODO: Update the log box periodically, limiting the contents too.
        //TODO: Provide option to follow tail (or follow tail when scrollbar near end?)

        #region Properties

        public Timer Timer { get; private set; }

        private RichTextBox _logBox;
        private MemoryAppender _memoryAppender;

        private RichTextBox LogBox
        {
            get
            {
                if (_logBox == null)
                {
                    var ctrl = Controls.Find("logBox", true);
                    if (ctrl.Length <= 0) throw new ApplicationException("Didn't find logBox!");
                    _logBox = (RichTextBox)ctrl[0];
                }
                return _logBox;
            }
        }

        private MemoryAppender MemoryAppender
        {
            get
            {
                if (_memoryAppender == null)
                {
                    var appenders = LogManager.GetRepository().GetAppenders();
                    foreach (var appender in appenders)
                    {
                        if (appender.Name != "MemoryAppender") continue;
                        _memoryAppender = (MemoryAppender)appender;
                        break;
                    }
                }
                return _memoryAppender;
            }
        }

        #endregion

        public LogControl()
        {
            InitializeComponent();
        }

        private void Log_Load(object sender, EventArgs e)
        {
            // Set the log box font.
            const string preferredFontName = "ProFontWindows";
            var testFont = new Font(preferredFontName, 12, FontStyle.Regular, GraphicsUnit.Pixel);
            LogBox.Font = testFont.Name == preferredFontName ? testFont : new Font("Courier New", 13, FontStyle.Regular, GraphicsUnit.Pixel);

            // Create timer for updating.
            Timer = new Timer { Interval = 10 };
            Timer.Tick += LogBoxTimer_Tick;
        }

        private void LogBoxTimer_Tick(object sender, EventArgs e)
        {
            UpdateLogBox();
        }

        public void ScrollToEnd()
        {
            LogBox.SelectionStart = LogBox.TextLength;
            LogBox.ScrollToCaret();
        }

        private void UpdateLogBox()
        {
            var events = MemoryAppender.GetEvents();
            if (events.Length <= 0) return;
            var sb = new StringBuilder();
            sb.Append(LogBox.Text);
            foreach (var logEvent in events)
            {
                var msg = logEvent.RenderedMessage;
                if (msg.StartsWith("CMD.INF: "))
                {
                    sb.Append(msg.Substring(9, msg.Length - 9));
                    sb.Append('\n');
                }
                else if (msg.StartsWith("CMD.ERR: "))
                {
                    sb.Append("ERROR! ").Append(msg.Substring(9, msg.Length - 9));
                    sb.Append('\n');
                }
            }
            MemoryAppender.Clear();
            LogBox.Text = sb.ToString();
            ScrollToEnd();
        }

        private void selectAllContextMenuItem_Click(object sender, EventArgs e)
        {
            LogBox.SelectAll();
            LogBox.Focus();
        }

        private void copyContextMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(LogBox.SelectedText);
        }

        private void clearContextMenuItem_Click(object sender, EventArgs e)
        {
            LogBox.Clear();
        }
    }
}
