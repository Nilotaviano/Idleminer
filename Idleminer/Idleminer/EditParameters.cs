using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Idleminer
{
    public partial class EditParameters : Form
    {
        public EditParameters(Parameters parameters)
        {
            Parameters = parameters;
            InitializeComponent();
        }

        public Parameters Parameters;

        protected override void OnLoad(EventArgs e)
        {
            txtPath.Text = Parameters.ExecutablePath;

            checkTimePicker.Value = new DateTime(
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                Parameters.IntervalBetweenChecks.Hours,
                Parameters.IntervalBetweenChecks.Minutes,
                Parameters.IntervalBetweenChecks.Seconds
            );

            idleTimePicker.Value = new DateTime(
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                Parameters.IdleTimeBeforeStarting.Hours,
                Parameters.IdleTimeBeforeStarting.Minutes,
                Parameters.IdleTimeBeforeStarting.Seconds
            ); ;
        }

        private void txtPath_TextChanged(object sender, EventArgs e)
        {
            Parameters.ExecutablePath = txtPath.Text;
        }

        private void checkTimePicker_ValueChanged(object sender, EventArgs e)
        {
            Parameters.IntervalBetweenChecks = new TimeSpan(checkTimePicker.Value.Hour, checkTimePicker.Value.Minute, checkTimePicker.Value.Second);
        }

        private void idleTimePicker_ValueChanged(object sender, EventArgs e)
        {
            Parameters.IdleTimeBeforeStarting = new TimeSpan(idleTimePicker.Value.Hour, idleTimePicker.Value.Minute, idleTimePicker.Value.Second);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (File.Exists(Parameters.ExecutablePath))
            {
                _UpdateSettings();

                this.DialogResult = DialogResult.OK;
                FormClosing -= EditParameters_FormClosing;
                Close();
            }
            else
            {
                MessageBox.Show("File not found.");
            }
        }

        private void _UpdateSettings()
        {
            Properties.Settings.Default.ExecutablePath = Parameters.ExecutablePath;
            Properties.Settings.Default.IdleTimeBeforeStarting = Parameters.IdleTimeBeforeStarting;
            Properties.Settings.Default.IntervalBetweenChecks = Parameters.IntervalBetweenChecks;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Necessary to prevent closing without click the "ok" button
        /// </summary>
        private void EditParameters_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                e.Cancel = true;
        }
    }
}
