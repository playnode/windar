using System.Text;

namespace Windar.Common
{
    public abstract class ShortCmd<T> : Cmd<T> where T : new()
    {
        readonly StringBuilder _stdOutput;
        readonly StringBuilder _stdErr;

        #region Properties

        public string Output
        {
            get { return _stdOutput.ToString(); }
        }

        public string Error
        {
            get { return _stdErr.ToString(); }
        }

        #endregion

        protected ShortCmd()
        {
            _stdOutput = new StringBuilder();
            _stdErr = new StringBuilder();
            Runner.CommandOutput += Cmd_CommandOutput;
            Runner.CommandError += Cmd_CommandError;
        }

        public abstract string Run();

        protected void Cmd_CommandOutput(object sender, CmdRunner.CommandEventArgs e)
        {
            _stdOutput.Append(e.Text).Append('\n');
        }

        protected void Cmd_CommandError(object sender, CmdRunner.CommandEventArgs e)
        {
            _stdErr.Append(e.Text).Append('\n');
        }
    }
}
