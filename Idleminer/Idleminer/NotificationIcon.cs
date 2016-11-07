using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Win32_API;

namespace Idleminer
{
    public partial class NotificationIcon : Form
    {
        private readonly TimeSpan timeToTrigger = new TimeSpan(hours: 0, minutes: 5, seconds: 0);
        private readonly TimeSpan intervalToCheck = new TimeSpan(hours: 0, minutes: 0, seconds: 5);

        public NotificationIcon(string path)
        {
            timer = new System.Timers.Timer(intervalToCheck.TotalMilliseconds);

            processInfo = new ProcessStartInfo(path)
            {
                CreateNoWindow = false
            };

            InitializeComponent();

            Disposed += _Disposed; ;
        }

        private System.Timers.Timer timer;

        private ProcessStartInfo processInfo;
        private Process process;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Hide();

            timer.Enabled = true;
            timer.Elapsed += Timer_Elapsed;
        }

        private void _KillProcess()
        {
            process?.CloseMainWindow();
            process?.WaitForExit();
            process = null;
        }

        private void Timer_Elapsed(object sender, EventArgs e)
        {
            var idleTime = Win32.GetIdleTime();

            if (idleTime < timeToTrigger.TotalMilliseconds)
            {
                _KillProcess();
            }
            else
            {
                if (process == null)
                {
                    try
                    {
                        process = Process.Start(processInfo);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "Error");
                        Close();
                    }
                }
                else
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }
        }

        private void _Disposed(object sender, EventArgs e)
        {
            _KillProcess();
            Close();
        }
    }
}
