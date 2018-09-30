using System.Text;
using Windar.Common;

namespace Windar.PlaydarDaemon.Commands
{
    class Status : ShortCmd<Status>
    {
        public override string Run()
        {
            Runner.SkipLogInfoOutput = true;

            Runner.RunCommand("CD " + DaemonController.Instance.Paths.PlaydarProgramPath);
            Runner.RunCommand("SET PLAYDAR_ETC=" + DaemonController.Instance.Paths.WindarAppData + @"\etc");

            StringBuilder cmd = new StringBuilder();
            cmd.Append('"').Append(DaemonController.Instance.Paths.ErlCmd).Append('"');
            cmd.Append(" -sname playdar-status@localhost");
            cmd.Append(" -noinput");
            cmd.Append(" -pa \"").Append(DaemonController.Instance.Paths.PlaydarProgramPath).Append("\\ebin\"");
            cmd.Append(" -s playdar_ctl");
            cmd.Append(" -extra playdar@localhost \"status\"");

            Runner.RunCommand(cmd.ToString());

            ContinueWhenDone();
            return Output;
        }
    }
}
