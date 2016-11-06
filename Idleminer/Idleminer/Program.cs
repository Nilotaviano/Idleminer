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
        private const string NO_PATH_SPECIFIED_MESSAGE = "No path specified";

        static void Main(string[] args)
        {
            if(args.Count() < 1)
            {
                Console.WriteLine(NO_PATH_SPECIFIED_MESSAGE);
                Console.ReadKey();

                return;
            }

            var timeToTrigger = new TimeSpan(hours: 0, minutes: 5, seconds: 0);
            var sleepTime = new TimeSpan(hours: 0, minutes: 1, seconds: 0);

            var processInfo = new ProcessStartInfo(args.First());
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            Process process = null; 

            do
            {
                var idleTime = Win32.GetIdleTime();

                if (idleTime < timeToTrigger.TotalMilliseconds)
                {
                    process?.Close();
                    process = null;

                    Thread.Sleep((int)sleepTime.TotalMilliseconds);
                }
                else
                {
                    if (process == null)
                    {
                        process = Process.Start(processInfo);

                        process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                            Console.WriteLine("output>>" + e.Data);
                        process.BeginOutputReadLine();

                        process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                            Console.WriteLine("error>>" + e.Data);
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
