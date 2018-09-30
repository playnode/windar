using System;
using System.Reflection;
using System.Threading;
using log4net;

namespace Windar.Common
{
    public abstract class Cmd<T> where T : new()
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().ReflectedType);

        CmdRunner _runner;
        bool _done;

        protected CmdRunner Runner
        {
            get { return _runner; }
            set { _runner = value; }
        }

        protected bool Done
        {
            get { return _done; }
            set { _done = value; }
        }

        protected Cmd()
        {
            Runner = new CmdRunner();
            Runner.CommandCompleted += Cmd_CommandCompleted;
        }

        public static T Create()
        {
            return new T();
        }

        protected void ContinueWhenDone()
        {
            if (Log.IsDebugEnabled) Log.Debug("Continue when done...");
            while (!Done) Thread.Sleep(100);
            if (Log.IsDebugEnabled) Log.Debug("Done.");
            Runner.Close();
            if (Log.IsDebugEnabled) Log.Debug("Runner closed.");
        }

        protected void Cmd_CommandCompleted(object sender, EventArgs e)
        {
            Done = true;
        }
    }
}
