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
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        #region Delegates and events.

        public class CommandEventArgs : EventArgs
        {
            public string Text;
            public CommandEventArgs(string text) { Text = text; }
        }

        public delegate void CommandOutputHandler(object sender, CommandEventArgs e);
        public delegate void CommandErrorHandler(object sender, CommandEventArgs e);
        public delegate void CommandCompletedHandler(object sender, EventArgs e);

        public event CommandOutputHandler CommandOutput;
        public event CommandErrorHandler CommandError;
        public event CommandCompletedHandler CommandCompleted;

        #endregion

        Thread _stdOutThread;
        Thread _stdErrThread;

        string _currCmdName;
        Process _process;
        bool _skipLogInfoOutput;

        public Process Process
        {
            get { return _process; }
            set { _process = value; }
        }

        public bool SkipLogInfoOutput
        {
            get { return _skipLogInfoOutput; }
            set { _skipLogInfoOutput = value; }
        }

        public CmdRunner()
        {
            Process = new Process();
            Process.StartInfo.WorkingDirectory = @"\";
            Process.StartInfo.FileName = "cmd.exe";
            Process.StartInfo.Arguments = "/K";
            Process.StartInfo.UseShellExecute = false;
            Process.StartInfo.CreateNoWindow = true;
            Process.StartInfo.RedirectStandardInput = true;
            Process.StartInfo.RedirectStandardOutput = true;
            Process.StartInfo.RedirectStandardError = true;
            Process.Start();
            Process.StandardInput.AutoFlush = true;
        }

        public void RunCommand(string cmd)
        {
            if (Log.IsInfoEnabled) Log.Info(string.Format("Run command: {0}", cmd));

            // If there is a command stll running, wait for standard out thread to finish.
            if (_stdOutThread != null)
            {
                if (Log.IsInfoEnabled) Log.Info("Previous command still running. Waiting.");
                _stdOutThread.Join();
                StopStandardOutputThread();
            }

            if (Log.IsInfoEnabled) Log.Info("Running command.");
            _currCmdName = GetCommandName(cmd);

            // Write commands to StandardInput.
            Process.StandardInput.WriteLine("prompt IGNORE: ");
            Process.StandardInput.WriteLine(cmd);
            Process.StandardInput.WriteLine("echo ---end of command---");
            Process.StandardInput.WriteLine();

            // Start reading from standard output.
            if (Process.StartInfo.RedirectStandardOutput)
            {
                _stdOutThread = new Thread(ReadStandardOutput);
                _stdOutThread.IsBackground = true;
                _stdOutThread.Start();
            }

            // Start reading from standard error.
            // We only create the standard error thread once.
            if (!Process.StartInfo.RedirectStandardError || _stdErrThread != null) return;
            _stdErrThread = new Thread(ReadStandardError);
            _stdErrThread.IsBackground = true;
            _stdErrThread.Start();
        }

        static string GetCommandName(string cmd)
        {
            if (cmd[0] != '"') return cmd;
            string result = cmd.Substring(1, cmd.Length - 1);
            result = result.Substring(0, result.IndexOf('"'));
            int pos = result.LastIndexOf('\\') + 1;
            return result.Substring(pos, result.Length - pos);
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
            for (int i = 0; i < 5; i++)
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

        void ReadStandardOutput()
        {
            try
            {
                // Ignore line: "prompt IGNORE: "
                string currentLine = Process.StandardOutput.ReadLine();
                if (Log.IsDebugEnabled) Log.Debug(string.Format("Ignoring line \"{0}\"", currentLine));

                // Ignoring blank line.
                currentLine = Process.StandardOutput.ReadLine();
                if (Log.IsDebugEnabled) Log.Debug(string.Format("Ignoring line \"{0}\"", currentLine));

                // Ignoring blank line.
                currentLine = Process.StandardOutput.ReadLine();
                if (Log.IsDebugEnabled) Log.Debug(string.Format("Ignoring line \"{0}\"", currentLine));

                // Build command output.
                currentLine = Process.StandardOutput.ReadLine();
                while (currentLine != null && currentLine != "---end of command---")
                {
                    if (!currentLine.StartsWith("IGNORE: "))
                    {
                        if (currentLine.Length > 0)
                        {
                            if (!SkipLogInfoOutput && Log.IsDebugEnabled) Log.Debug(string.Format("CMD.INF: [{0}] {1}", _currCmdName, currentLine));
                            if (CommandOutput != null) CommandOutput(this, new CommandEventArgs(currentLine));
                        }
                    }
                    else if (Log.IsDebugEnabled)
                    {
                        if (string.IsNullOrEmpty(currentLine)) Log.Debug("Ignoring empty line.");
                        else Log.Debug(string.Format("Ignoring line \"{0}\"", currentLine));
                    }
                    currentLine = Process.StandardOutput.ReadLine();
                }
            }
            catch (ThreadAbortException)
            {
                if (Log.IsWarnEnabled) Log.Warn("StandardOutput: Thread aborted.");
            }

            //TODO: _stdErr = null; // ?

            // Call method to signal that command has completed.
            if (CommandCompleted != null) CommandCompleted(this, new EventArgs());
            if (Log.IsDebugEnabled) Log.Debug("Command completed.");            
        }

        void ReadStandardError()
        {
            try
            {
                StringBuilder line = new StringBuilder();
                do
                {
                    int c = Process.StandardError.Read();
                    if (c == 13) continue;
                    if (c != 10) line.Append((char) c);
                    else
                    {
                        if (Log.IsDebugEnabled) Log.Debug(string.Format("CMD.ERR: [{0}] {1}", _currCmdName, line));
                        if (CommandError != null) CommandError(this, new CommandEventArgs(line.ToString()));
                        line = new StringBuilder();
                    }

                } while (Process != null && !Process.StandardError.EndOfStream);
            }
            catch (ThreadAbortException)
            {
                if (Log.IsWarnEnabled) Log.Warn("StandardError: Thread aborted.");
            }
        }

        void StopStandardOutputThread()
        {
            // Try to abort the standard output reader.
            for (int i = 0; i < 50; i++)
            {
                if (_stdOutThread == null || !_stdOutThread.IsAlive) 
                    return;
                
                Thread.Sleep(100);
            }
            if (Log.IsWarnEnabled) 
                Log.Warn("Aborting reader thread for standard output.");
            try
            {
                _stdOutThread.Interrupt();
                _stdOutThread.Abort();
            }
            catch (Exception ex)
            {
                if (Log.IsErrorEnabled) 
                    Log.Error("Exception", ex);
            }
            _stdOutThread = null;
        }

        void StopStandardErrorThread()
        {
            // Try to abort the standard error reader.
            for (int i = 0; i < 50; i++)
            {
                if (_stdErrThread == null || !_stdErrThread.IsAlive) 
                    return;
                
                Thread.Sleep(100);
            }
            if (Log.IsWarnEnabled) 
                Log.Warn("Aborting reader thread for error output.");
            try
            {
                _stdErrThread.Interrupt();
                _stdErrThread.Abort();
            }
            catch (Exception ex)
            {
                if (Log.IsErrorEnabled) 
                    Log.Error("Exception", ex);
            }
            _stdErrThread = null;
        }
    }
}
