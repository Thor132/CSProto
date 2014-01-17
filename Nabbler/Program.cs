// Screenshots all process windows matching the passed in process name.
// If the window is minimized, it will be restored during the screenshot.
namespace Nabbler
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using CommandLine;
    using CommandLine.Text;

    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public int Width { get { return right - left; } }
        public int Height { get { return bottom - top; } }
    }

    public class Options
    {
        [Option('p', "proc", Required = true, HelpText = "Process name to screenshot.")]
        public string ProcessName { get; set; }

        [Option('o', "out", Required = true, HelpText = "Output directory.")]
        public string OutputDirectory { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            if (Parser.Default.ParseArguments(args, options))
            {
                List<Process> processes = Process.GetProcessesByName(options.ProcessName).ToList();
                if (!processes.Any())
                {
                    Console.WriteLine("No processes were found with name {0}", options.ProcessName);
                }

                DirectoryInfo outputDirectory = new DirectoryInfo(options.OutputDirectory);
                if (!outputDirectory.Exists)
                {
                    outputDirectory.Create();
                }

                foreach (Process process in processes)
                {
                    Console.WriteLine("Capturing {0} ID {1}", process.ProcessName, process.Id);
                    Bitmap bmp = PrintWindow(process.MainWindowHandle);
                    string outputPath = Path.Combine(options.OutputDirectory, string.Format("{0}-{1}.png", process.ProcessName, process.Id));
                    bmp.Save(outputPath, ImageFormat.Png);
                    Console.WriteLine("Saved to {0}", outputPath);
                }
            }
        }

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out Rect lpRect);
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);
        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);
        [DllImport("user32.dll")]
        private static extern IntPtr ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        private const int GWL_STYLE = -16;
        private const int SW_HIDE = 0;
        private const int SW_RESTORE = 9;
        private const int SW_MINIMIZE = 6;
        private const uint WS_MINIMIZE = 0x20000000;

        public static Bitmap PrintWindow(IntPtr hwnd)
        {
            bool wasMinimized = false;
            int style = GetWindowLong(hwnd, GWL_STYLE);
            if ((style & WS_MINIMIZE) == WS_MINIMIZE)
            {
                // Needed to add an explicit hide due to not working properly with the steam window.
                ShowWindow(hwnd, SW_HIDE);
                ShowWindow(hwnd, SW_RESTORE);
                wasMinimized = true;
            }

            Rect rect;
            GetWindowRect(hwnd, out rect);

            // http://stackoverflow.com/questions/891345/get-a-screenshot-of-a-specific-application
            Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            Graphics gfxBmp = Graphics.FromImage(bmp);
            IntPtr hdcBitmap = gfxBmp.GetHdc();

            PrintWindow(hwnd, hdcBitmap, 0);

            gfxBmp.ReleaseHdc(hdcBitmap);
            gfxBmp.Dispose();

            if (wasMinimized)
            {
                ShowWindow(hwnd, SW_MINIMIZE);
            }

            return bmp;
        }
    }
}
