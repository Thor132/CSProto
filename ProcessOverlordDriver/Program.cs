using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ProcessOverlordDriver
{
    class Program
    {
        static void Main(string[] args)
        {
            Process managed0 = new Process();
            managed0.StartInfo.FileName = @"notepad";
            managed0.Start();
            
            Process managed1 = new Process();
            managed1.StartInfo.FileName = @"calc";
            managed1.Start();

            Process managed2 = new Process();
            managed2.StartInfo.FileName = @"calc";
            managed2.Start();

            Process overlord = new Process();
            overlord.StartInfo.FileName = @"..\..\..\ProcessOverlord\bin\Debug\ProcessOverlord.exe";
            overlord.StartInfo.Arguments = string.Format("{0} {1} {2} {3}", Process.GetCurrentProcess().Id, managed0.Id, managed1.Id, managed2.Id);
            overlord.Start();

            while (true)
            {
                string input = Console.ReadLine();
                if (input == "exit")
                {
                    break;
                }
            }
        }
    }
}
