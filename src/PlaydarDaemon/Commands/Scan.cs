using System;
using System.Text;
using Windar.Common;

namespace Windar.PlaydarDaemon.Commands
{
    class Scan : AsyncCmd<Scan>
    {
        public delegate void ScanCompletedHandler(object sender, EventArgs e);

        public event ScanCompletedHandler ScanCompleted;

        bool _firstRun;
        string _scanPath;

        public string ScanPath
        {
            get { return _scanPath; }
            set { _scanPath = value; }
        }

        public override void RunAsync()
        {
            if (ScanPath == null) throw new ApplicationException("ScanPath must be defined");

            Runner.CommandCompleted += Completed;

            Runner.RunCommand(@"CD " + DaemonController.Instance.Paths.WindarAppData);

            StringBuilder cmd = new StringBuilder();
            cmd.Append('"').Append(DaemonController.Instance.Paths.ErlCmd).Append('"');
            cmd.Append(" -sname playdar-scan@localhost");
            cmd.Append(" -noinput");
            cmd.Append(" -pa \"").Append(DaemonController.Instance.Paths.PlaydarProgramPath).Append("\\ebin\"");
            cmd.Append(" -s playdar_ctl");
            cmd.Append(" -extra playdar@localhost \"scan\" \"");
            cmd.Append(ScanPath).Append("\"");

            Runner.RunCommand(cmd.ToString());
        }

        protected void Completed(object sender, EventArgs e)
        {
            //NOTE: This event first at start for some reason. Ignore first.
            if (!_firstRun) _firstRun = true;
            else ScanCompleted(this, new EventArgs());
        }
    }
}
