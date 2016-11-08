using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Idleminer
{
    public struct Parameters
    {
        public Parameters(string executablePath = null, TimeSpan? idleTimeBeforeStarting = null, TimeSpan? intervalBetweenChecks = null)
        {
            ExecutablePath = executablePath ?? Properties.Settings.Default.ExecutablePath;
            IdleTimeBeforeStarting = idleTimeBeforeStarting ?? Properties.Settings.Default.IdleTimeBeforeStarting;
            IntervalBetweenChecks = intervalBetweenChecks ?? Properties.Settings.Default.IntervalBetweenChecks;
        }

        /// <summary>
        /// Path of the executable to be run when idle
        /// </summary>
        public string ExecutablePath { get; set; }

        /// <summary>
        /// Idle time before starting to mine
        /// </summary>
        public TimeSpan IdleTimeBeforeStarting { get; set; }

        /// <summary>
        /// Interval between checks for user input
        /// </summary>
        public TimeSpan IntervalBetweenChecks { get; set; }
    }
}
