using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ProcessOverlord
{
    class Program
    {
        static int Main(string[] args)
        {
            List<Process> managed = new List<Process>();
            Process tracked = null;

            if (args.Length < 2)
            {
                Console.WriteLine("Error: Requires at least two PIDs as arguments.");
                return 1;
            }

            foreach (string arg in args)
            {
                Process process = GetProcessFromIdString(arg);
                if (process != null)
                {
                    if (tracked == null)
                    {
                        tracked = process;
                        Console.WriteLine("Tracking {0} pid {1}", process.ProcessName, process.Id);
                    }
                    else
                    {
                        managed.Add(process);
                        Console.WriteLine("Managing {0} pid {1}", process.ProcessName, process.Id);
                    }
                }
            }

            if (tracked == null || !managed.Any())
            {
                Console.WriteLine("Error: Unable to find processes from provided arguments.");
                return 1;
            }

            while (true)
            {
                if (tracked.HasExited)
                {
                    Console.WriteLine("Tracked process has exited.");
                    KillProcesses(managed);
                    break;
                }

                Thread.Sleep(1000);
            }

            return 0;
        }

        private static void KillProcesses(List<Process> processes)
        {
            Console.WriteLine("Killing managed processes.");

            foreach (Process process in processes)
            {
                try
                {
                    process.Kill();
                }
                catch
                {
                }
            }
        }

        private static Process GetProcessFromIdString(string id)
        {
            Process process = null;
            int pid;
            if (int.TryParse(id, out pid))
            {
                process = Process.GetProcessById(pid);
            }

            return process;
        }
    }
}