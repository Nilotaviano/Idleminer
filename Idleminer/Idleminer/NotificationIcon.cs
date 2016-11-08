using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Win32_API;

namespace Idleminer
{
    public partial class NotificationIcon : Form
    {
        public NotificationIcon(Parameters parameters)
        {
            _Parameters = parameters;

            _Timer = new System.Timers.Timer(parameters.IntervalBetweenChecks.TotalMilliseconds);

            InitializeComponent();

            Disposed += _Disposed; ;
        }

        private Parameters _Parameters;

        private System.Timers.Timer _Timer;

        private ProcessStartInfo _ProcessInfo;
        private Process _Process;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // hide window so the program runs only on the taskbar
            Hide();

            if (string.IsNullOrWhiteSpace(_Parameters.ExecutablePath))
            {
                _ConfigureParameters();

                // If the user didn't configure a file to be executed, then close the program
                if (string.IsNullOrWhiteSpace(_Parameters.ExecutablePath))
                    _Close();
            }

            _CreateProcessInfo();

            _Timer.Enabled = true;
            _Timer.Elapsed += Timer_Elapsed;
        }

        private void _CreateProcessInfo()
        {
            _ProcessInfo = new ProcessStartInfo(_Parameters.ExecutablePath)
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
            var editParametersForm = new EditParameters(_Parameters);

            if (editParametersForm.ShowDialog() == DialogResult.OK)
            {
                _Parameters = editParametersForm.Parameters;
                _Timer.Interval = _Parameters.IntervalBetweenChecks.TotalMilliseconds;
                _CreateProcessInfo();
            }
        }

        private void Timer_Elapsed(object sender, EventArgs e)
        {
            _Timer.Enabled = false;

            var idleTime = Win32.GetIdleTime();

            if (idleTime < _Parameters.IdleTimeBeforeStarting.TotalMilliseconds)
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

            _Timer.Enabled = true;
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
