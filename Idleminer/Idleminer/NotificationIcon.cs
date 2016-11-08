using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Win32_API;

namespace Idleminer
{
    public partial class NotificationIcon : Form
    {
        public NotificationIcon(string path)
        {
            _Path = path;

            _Timer = new System.Timers.Timer(_IntervalToCheck.TotalMilliseconds);

            InitializeComponent();

            Disposed += _Disposed; ;
        }

        private System.Timers.Timer _Timer;

        private ProcessStartInfo _ProcessInfo;
        private Process _Process;

        private string _Path;

        private TimeSpan _TimeToTrigger = new TimeSpan(hours: 0, minutes: 5, seconds: 0);
        private TimeSpan _IntervalToCheck = new TimeSpan(hours: 0, minutes: 0, seconds: 5);

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Hide();

            if (string.IsNullOrWhiteSpace(_Path))
                _ConfigureParameters();

            // If the user didn't configure a file to be executed, then close the program
            if (string.IsNullOrWhiteSpace(_Path))
                _Close();

            _CreateProcessInfo();

            _Timer.Enabled = true;
            _Timer.Elapsed += Timer_Elapsed;
        }

        private void _CreateProcessInfo()
        {
            _ProcessInfo = new ProcessStartInfo(_Path)
            {
                CreateNoWindow = false
            };
        }

        private void _KillProcess()
        {
            _Process?.CloseMainWindow();
            _Process?.WaitForExit();
            _Process = null;
        }

        private void _ConfigureParameters()
        {
            var form = new EditParameters(_Path, _TimeToTrigger, _IntervalToCheck);

            if (form.ShowDialog() == DialogResult.OK)
            {
                _Path = form.Path;
                _TimeToTrigger = form.TimeToTrigger;
                _IntervalToCheck = form.IntervalToCheck;
                _CreateProcessInfo();
                _Timer.Interval = _IntervalToCheck.TotalMilliseconds;
            }
        }

        private void Timer_Elapsed(object sender, EventArgs e)
        {
            var idleTime = Win32.GetIdleTime();

            if (idleTime < _TimeToTrigger.TotalMilliseconds)
            {
                _KillProcess();
            }
            else
            {
                if (_Process == null)
                {
                    try
                    {
                        _Process = Process.Start(_ProcessInfo);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error");
                        _Close();
                    }
                }
            }
        }

        private void _Disposed(object sender, EventArgs e)
        {
            _Close();
        }

        private void _Close()
        {
            _KillProcess();
            Close();
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _KillProcess();
            _Timer.Enabled = false;
            pauseToolStripMenuItem.Visible = false;
            resumeToolStripMenuItem.Visible = true;
        }

        private void resumeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _Timer.Enabled = true;
            pauseToolStripMenuItem.Visible = true;
            resumeToolStripMenuItem.Visible = false;
        }

        private void parametersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ConfigureParameters();
        }
    }
}
