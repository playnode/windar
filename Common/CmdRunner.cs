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
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading;
using log4net;

namespace Windar.Common
{
    public class CmdRunner
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        #region Delegates and Events

        public delegate void CommandOutputHandler(object sender, CommandEventArgs e);
        public delegate void CommandErrorHandler(object sender, CommandEventArgs e);
        public delegate void CommandCompletedHandler(object sender, CommandCompletedEventArgs e);

        public class CommandEventArgs : EventArgs
        {
            public string Text;
            public CommandEventArgs(string text) { Text = text; }
        }

        public class CommandCompletedEventArgs : CommandEventArgs
        {
            public readonly string ErrorText;
            public CommandCompletedEventArgs(string text, string errorText)
                : base(text)
            {
                ErrorText = errorText;
            }
        }

        public event CommandOutputHandler CommandOutput;
        public event CommandErrorHandler CommandError;
        public event CommandCompletedHandler CommandCompleted;

        #endregion

        private Thread _stdOutThread;
        private Thread _stdErrThread;
        private StringBuilder _stdErr;

        public Process Process { get; private set; }

        public CmdRunner()
        {
            Process = new Process
                           {
                               StartInfo =
                                   {
                                       WorkingDirectory = @"\",
                                       FileName = "cmd.exe",
                                       Arguments = "/K",
                                       UseShellExecute = false,
                                       CreateNoWindow = true,
                                       RedirectStandardInput = true,
                                       RedirectStandardOutput = true,
                                       RedirectStandardError = true
                                   }
                           };

            Process.Start();
            Process.StandardInput.AutoFlush = true;
        }

        public void RunCommand(string cmd)
        {
            if (Log.IsInfoEnabled) Log.Info("Run command: " + cmd);

            // If there is a command stll running, wait for standard out thread to finish.
            if (_stdOutThread != null)
            {
                if (Log.IsInfoEnabled) Log.Info("Previous command still running. Waiting.");
                _stdOutThread.Join();
                StopStandardOutputThread();
            }

            // Initialise standard error string builder.
            _stdErr = new StringBuilder();

            if (Log.IsInfoEnabled) Log.Info("Running command.");

            // Write commands to StandardInput.
            Process.StandardInput.WriteLine("prompt PROMPT:");
            Process.StandardInput.WriteLine(cmd);
            Process.StandardInput.WriteLine("echo ---end of command---");

            // Start reading from standard output.
            if (Process.StartInfo.RedirectStandardOutput)
            {
                _stdOutThread = new Thread(ReadStandardOutput) {IsBackground = true};
                _stdOutThread.Start();
            }

            // Start reading from standard error.
            // We only create the standard error thread once.
            if (!Process.StartInfo.RedirectStandardError || _stdErrThread != null) return;
            _stdErrThread = new Thread(ReadStandardError) {IsBackground = true};
            _stdErrThread.Start();
        }

        public void Close()
        {
            RunCommand("exit");

            // Stop the threads collecting the outputs.
            StopStandardOutputThread();
            StopStandardErrorThread();

            // Kill the CmdRunner process if not exiting.
            if (Process.HasExited) return;
            if (Log.IsWarnEnabled) Log.Warn("CmdRunner closing but process has not yet exited. Giving it a little more time.");
            for (var i = 0; i < 5; i++)
            {
                if (!Process.HasExited)
                {
                    Thread.Sleep(1000);
                }
                else
                {
                    break;
                }
            }
            if (!Process.HasExited)
            {
                if (Log.IsWarnEnabled) Log.Warn("Killing the CmdRunner process now!");
                Process.Kill();
            }
            Process = null;
        }

        #region Private methods

        private void OnCommandOutput(CommandEventArgs e)
        {
            if (CommandOutput != null) CommandOutput(this, e);
        }

        private void OnCommandError(CommandEventArgs e)
        {
            if (CommandError != null) CommandError(this, e);
        }

        private void OnCommandCompleted(CommandCompletedEventArgs e)
        {
            if (CommandCompleted != null) CommandCompleted(this, e);
        }

        private void ReadStandardOutput()
        {
            var output = new StringBuilder();
            try
            {
                // Ignore line: "prompt PROMPT:"
                var currentLine = Process.StandardOutput.ReadLine();
                if (Log.IsDebugEnabled) Log.Debug("Ignoring line \"" + currentLine + '"');

                // Ignoring blank line.
                currentLine = Process.StandardOutput.ReadLine();
                if (Log.IsDebugEnabled) Log.Debug("Ignoring line \"" + currentLine + '"');

                // Ignoring blank line.
                currentLine = Process.StandardOutput.ReadLine();
                if (Log.IsDebugEnabled) Log.Debug("Ignoring line \"" + currentLine + '"');

                // Build command output.
                currentLine = Process.StandardOutput.ReadLine();
                while (currentLine != null && currentLine != "---end of command---")
                {
                    if (!currentLine.StartsWith("PROMPT:"))
                    {
                        if (currentLine.Length > 0)
                        {
                            if (Log.IsDebugEnabled) Log.Debug(currentLine);
                            OnCommandOutput(new CommandEventArgs(currentLine));
                        }
                        output.Append('\n').Append(currentLine);
                    }
                    else if (Log.IsDebugEnabled) Log.Debug("Ignoring line \"" + currentLine + '"');
                    currentLine = Process.StandardOutput.ReadLine();
                }
            }
            catch (ThreadAbortException)
            {
                if (Log.IsWarnEnabled) Log.Warn("StandardOutput: Thread aborted.");
            }

            // Call method to signal that command has completed.
            OnCommandCompleted(new CommandCompletedEventArgs(output.ToString(), _stdErr.ToString()));
            //TODO: _stdErr = null; // ?
        }

        private void ReadStandardError()
        {
            try
            {
                var line = new StringBuilder();
                do
                {
                    var c = Process.StandardError.Read();
                    if (c == 13) continue;
                    _stdErr.Append((char) c);
                    if (c != 10) line.Append((char) c);
                    else
                    {
                        if (Log.IsDebugEnabled) Log.Debug(line.ToString());
                        OnCommandError(new CommandEventArgs(line.ToString()));
                        line = new StringBuilder();
                    }

                } while (Process != null && !Process.StandardError.EndOfStream);
            }
            catch (ThreadAbortException)
            {
                if (Log.IsWarnEnabled) Log.Warn("StandardError: Thread aborted.");
            }
        }

        private void StopStandardOutputThread()
        {
            // Try to abort the standard output reader.
            if (_stdOutThread == null || !_stdOutThread.IsAlive) return;
            for (var i = 0; i < 5; i++)
            {
                Thread.Sleep(1000);
                if (_stdOutThread == null || !_stdOutThread.IsAlive) return;
            }
            if (Log.IsWarnEnabled) Log.Warn("Aborting reader thread for standard output.");
            try
            {
                _stdOutThread.Interrupt();
                _stdOutThread.Abort();
            }
            catch (Exception ex)
            {
                if (Log.IsErrorEnabled) Log.Error("Exception", ex);
            }
            _stdOutThread = null;
        }

        private void StopStandardErrorThread()
        {
            // Try to abort the standard error reader.
            if (_stdErrThread == null || !_stdErrThread.IsAlive) return;
            for (var i = 0; i < 5; i++)
            {
                Thread.Sleep(1000);
                if (_stdOutThread == null || !_stdOutThread.IsAlive) return;
            }
            if (Log.IsWarnEnabled) Log.Warn("Aborting reader thread for error output.");
            try
            {
                _stdErrThread.Interrupt();
                _stdErrThread.Abort();
            }
            catch (Exception ex)
            {
                if (Log.IsErrorEnabled) Log.Error("Exception", ex);
            }
            _stdErrThread = null;
        }

        #endregion
    }
}