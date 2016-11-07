using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Win32_API;

namespace Idleminer
{
    class Program
    {
        private const string NO_PATH_SPECIFIED_MESSAGE = "Specify the path: ";

        static void Main(string[] args)
        {
            string path = args.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(path))
            {
                Console.WriteLine(NO_PATH_SPECIFIED_MESSAGE);
                path = Console.ReadLine();
            }

            var timeToTrigger = new TimeSpan(hours: 0, minutes: 5, seconds: 0);
            var sleepTime = new TimeSpan(hours: 0, minutes: 1, seconds: 0);

            var processInfo = new ProcessStartInfo(path)
            {
                CreateNoWindow = false
            };

            Process process = null;

            do
            {
                var idleTime = Win32.GetIdleTime();

                if (idleTime < timeToTrigger.TotalMilliseconds)
                {
                    process?.CloseMainWindow();
                    process?.WaitForExit();
                    process = null;

                    Thread.Sleep((int)sleepTime.TotalMilliseconds);
                }
                else
                {
                    if (process == null)
                    {
                        process = Process.Start(processInfo);
                    }
                    else
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                    }
                }

            } while (true);
        }
    }
}
