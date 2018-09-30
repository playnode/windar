using System.Windows.Forms;

namespace Windar.TrayApp
{
    public partial class WaitingDialog : Form
    {
        public delegate void DoMethod();

        DoMethod _do;

        public DoMethod Do
        {
            get { return _do; }
            set { _do = value; }
        }

        bool _run;

        public WaitingDialog()
        {
            InitializeComponent();
        }

        void SetRun(bool run)
        {
            _run = run;
            Program.Instance.Tray.ToggleMainFormOptions(!_run);
        }

        public void Stop()
        {
            SetRun(false);
        }

        void WaitingDialog_Shown(object sender, System.EventArgs e)
        {
            SetRun(true);
            if (Do != null) backgroundWorker.RunWorkerAsync();
            while (_run)
            {
                progressBar.Update();
                Application.DoEvents();
                System.Threading.Thread.Sleep(100);
            }
            Close();
        }

        void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Do();
        }

        void backgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            SetRun(false);
        }
    }
}
