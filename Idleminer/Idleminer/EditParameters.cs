using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Idleminer
{
    public partial class EditParameters : Form
    {
        public EditParameters(string path, TimeSpan timeToTrigger, TimeSpan intervalToCheck)
        {
            Path = path;
            TimeToTrigger = timeToTrigger;
            IntervalToCheck = intervalToCheck;

            InitializeComponent();
        }

        public string Path { get; set; }
        public TimeSpan TimeToTrigger { get; set; }
        public TimeSpan IntervalToCheck { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            txtPath.Text = Path;
            checkTimePicker.Value = new DateTime(
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                IntervalToCheck.Hours, 
                IntervalToCheck.Minutes, 
                IntervalToCheck.Seconds
            );

            idleTimePicker.Value = new DateTime(
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                TimeToTrigger.Hours,
                TimeToTrigger.Minutes,
                TimeToTrigger.Seconds
            ); ;
        }

        private void txtPath_TextChanged(object sender, EventArgs e)
        {
            Path = txtPath.Text;
        }

        private void checkTimePicker_ValueChanged(object sender, EventArgs e)
        {
            IntervalToCheck = new TimeSpan(checkTimePicker.Value.Hour, checkTimePicker.Value.Minute, checkTimePicker.Value.Second);
        }

        private void idleTimePicker_ValueChanged(object sender, EventArgs e)
        {
            TimeToTrigger = new TimeSpan(idleTimePicker.Value.Hour, idleTimePicker.Value.Minute, idleTimePicker.Value.Second);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}
