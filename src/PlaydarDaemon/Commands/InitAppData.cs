using System.Text;
using Windar.Common;

namespace Windar.PlaydarDaemon.Commands
{
    class InitAppData : ShortCmd<InitAppData>
    {
        public override string Run()
        {
            Runner.SkipLogInfoOutput = false;

            // Path to %AppData%\Windar
            StringBuilder cmd = new StringBuilder();
            cmd.Append("IF NOT EXIST \"").Append(DaemonController.Instance.Paths.WindarAppData).Append("\" ");
            cmd.Append("MKDIR \"").Append(DaemonController.Instance.Paths.WindarAppData).Append("\\\"");
            Runner.RunCommand(cmd.ToString());

            // Path to %AppData%\Windar\etc
            cmd = new StringBuilder();
            cmd.Append("IF NOT EXIST \"").Append(DaemonController.Instance.Paths.WindarAppData).Append("\\etc\" ");
            cmd.Append("MKDIR \"").Append(DaemonController.Instance.Paths.WindarAppData).Append("\\etc\\\"");
            Runner.RunCommand(cmd.ToString());

            // Playdar configuration file.
            cmd = new StringBuilder();
            cmd.Append("IF NOT EXIST \"").Append(DaemonController.Instance.Paths.WindarAppData).Append("\\etc\\playdar.conf\" ");
            cmd.Append("COPY \"").Append(DaemonController.Instance.Paths.PlaydarProgramPath).Append("\\etc\\playdar.conf\"");
            cmd.Append(" \"").Append(DaemonController.Instance.Paths.WindarAppData).Append("\\etc\\\"");
            Runner.RunCommand(cmd.ToString());

            // Playdar TCP configuration file.
            cmd = new StringBuilder();
            cmd.Append("IF NOT EXIST \"").Append(DaemonController.Instance.Paths.WindarAppData).Append("\\etc\\playdartcp.conf\" ");
            cmd.Append("COPY \"").Append(DaemonController.Instance.Paths.PlaydarProgramPath).Append("\\etc\\playdartcp.conf\"");
            cmd.Append(" \"").Append(DaemonController.Instance.Paths.WindarAppData).Append("\\etc\\\"");
            Runner.RunCommand(cmd.ToString());

            ContinueWhenDone();
            return Output;
        }
    }
}
