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
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using log4net;
using log4net.Appender;

namespace Windar.TrayApp
{
    partial class LogControl : UserControl
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        private const int MaxBufferSize = 1024 * 256; // 256K

        private string _buffer;
        private bool _bufferChanged;
        private int _linesRemoved;
        private int _lineHeight;
        private bool _updating;
        private Timer _timer;

        #region Properties

        private MemoryAppender _memoryAppender;
        private RichTextBoxPlus _logBox;

        public RichTextBoxPlus LogBox
        {
            get
            {
                if (_logBox == null)
                {
                    var ctrl = Controls.Find("logBox", true);
                    if (ctrl.Length <= 0) throw new ApplicationException("Didn't find logBox!");
                    _logBox = (RichTextBoxPlus) ctrl[0];
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

        #region Init

        public LogControl()
        {
            InitializeComponent();
        }

        private void Log_Load(object sender, EventArgs e)
        {
            // Set the log box font.
            const string preferredFontName = "ProFontWindows";
            var testFont = new Font(preferredFontName, 12, FontStyle.Regular, GraphicsUnit.Pixel);
            LogBox.Font = testFont.Name == preferredFontName ? testFont : 
                new Font("Courier New", 13, FontStyle.Regular, GraphicsUnit.Pixel);

            // Store line-height.
            _lineHeight = testFont.Name == preferredFontName ? 12 : 13;

            // Create timer for updating.
            _timer = new Timer { Interval = 10 };
            _timer.Tick += LogBoxTimer_Tick;

            // First update.
            _timer.Start();
            UpdateOutputBuffer();
            LogBox.SetText(_buffer, _linesRemoved, _lineHeight);
        }

        #endregion

        public void StartUpdating()
        {
            _updating = true;
        }

        public void StopUpdating()
        {
            _updating = false;
        }

        private void UpdateOutputBuffer()
        {
            var sb = new StringBuilder();
            try
            {
                var events = MemoryAppender.GetEvents();
                if (events.Length <= 0) return;
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
            }
            catch (Exception ex)
            {
                if (Log.IsErrorEnabled) Log.Error("Exception when reading log memory appender.", ex);
                _buffer = null;
            }

            // Clear appender until next update.
            MemoryAppender.Clear();

            // Update the log buffer.
            if (sb.Length > 0) sb.Remove(sb.Length - 1, 1); // Remove last newline.
            if (sb.Length > 0) _bufferChanged = true;
            var s = sb.ToString();
            sb = new StringBuilder();
            if (!string.IsNullOrEmpty(_buffer)) sb.Append(_buffer).Append('\n');
            sb.Append(s);

            // Remove lines from beginning of buffer if necessary.
            _linesRemoved = sb.Length <= MaxBufferSize ? _linesRemoved : 
                _linesRemoved + TrimBuffer(sb, MaxBufferSize);

            _buffer = sb.ToString();
        }

        private static int TrimBuffer(StringBuilder sb, int size)
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
            _timer.Stop();
        }

        #region Event handlers.

        private void LogBoxTimer_Tick(object sender, EventArgs e)
        {
            UpdateOutputBuffer();
            if (_updating && _bufferChanged) LogBox.SetText(_buffer, _linesRemoved, _lineHeight);

            // Reset the following buffer-related vars.
            _bufferChanged = false;
            _linesRemoved = 0;
        }

        private void copyContextMenuItem_Click(object sender, EventArgs e)
        {
            if (LogBox.SelectedText.Length > 0) Clipboard.SetText(LogBox.SelectedText);
            else Clipboard.SetText(LogBox.Text);
        }

        private void clearContextMenuItem_Click(object sender, EventArgs e)
        {
            _buffer = null;
            LogBox.Clear();
        }

        #endregion
    }
}
