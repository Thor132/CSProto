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
    using System.Text.RegularExpressions;
    using System.Threading;
    using CommandLine;
    using CommandLine.Text;

    [StructLayout(LayoutKind.Sequential)]
    internal struct Rect
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public int Width { get { return right - left; } }
        public int Height { get { return bottom - top; } }
    }

    internal static class Native
    {
        internal enum ShowWindowCommand : int { SW_HIDE = 0, SW_MINIMIZE = 6, SW_RESTORE = 9 }
        internal enum RasterOperation : uint { SRC_COPY = 0x00CC0020 }
        internal enum KeyCodes : int { VK_LMENU = 0xA4 }
        [Flags]
        internal enum KeyboardFlags : int { KEYEVENTF_EXTENDEDKEY = 0x1, KEYEVENTF_KEYUP = 0x2 }

        [DllImport("user32.dll")]
        internal static extern bool GetClientRect(IntPtr hWnd, out Rect lpRect);
        [DllImport("user32.dll")]
        internal static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("user32.dll")]
        internal static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);
        [DllImport("gdi32.dll")]
        internal static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, RasterOperation rasterOperation);

        [DllImport("user32.dll")]
        internal static extern bool IsIconic(IntPtr hWnd);
        [DllImport("user32.dll")]
        internal static extern IntPtr ShowWindow(IntPtr hWnd, ShowWindowCommand nCmdShow);
        [DllImport("user32.dll")]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll")]
        internal static extern int SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        internal static extern bool AllowSetForegroundWindow(int dwProcessId);
        [DllImport("user32.dll")]
        internal static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
        [DllImport("user32.dll")]
        internal static extern void keybd_event(byte bVk, byte bScan, KeyboardFlags dwFlags, int dwExtraInfo);
    }

    public class Options
    {
        [Option('p', "proc", Required = true, HelpText = "Process name regular expression.")]
        public string ProcessNameRegex { get; set; }

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
        private const int RestoreWaitTime = 500;

        private static void Main(string[] args)
        {
            var options = new Options();
            if (Parser.Default.ParseArguments(args, options))
            {
                Regex regex = new Regex(options.ProcessNameRegex, RegexOptions.IgnoreCase);
                List<Process> processes = Process.GetProcesses().Where(p => regex.IsMatch(p.ProcessName)).ToList();
                if (!processes.Any())
                {
                    Console.WriteLine("No processes were found with the regular expression \"{0}\"", options.ProcessNameRegex);
                }

                DirectoryInfo outputDirectory = new DirectoryInfo(options.OutputDirectory);
                if (!outputDirectory.Exists)
                {
                    outputDirectory.Create();
                }

                foreach (Process process in processes)
                {
                    try
                    {
                        Console.WriteLine("Capturing {0}({1})", process.ProcessName, process.Id);
                        Bitmap bmp = CaptureProcessMainWindow(process);
                        if (bmp == null)
                        {
                            Console.WriteLine("An error occurred when attempting to capture screenshot.");
                            continue;
                        }

                        string outputPath = Path.Combine(options.OutputDirectory, string.Format("{0}.{1}.png", process.ProcessName, process.Id));
                        bmp.Save(outputPath, ImageFormat.Png);
                        Console.WriteLine("Saved to {0}", outputPath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An exception occurred for process {0}({1}): {2}", process.ProcessName, process.Id, ex.Message);
                    }
                }
            }
        }

        public static Bitmap CaptureProcessMainWindow(Process process)
        {
            IntPtr hwnd = process.MainWindowHandle;

            RestoreWindow(hwnd);
            SetForgroundWindowMethod1(process);

            Rect rect;
            Native.GetClientRect(hwnd, out rect);
            if (rect.Width == 0 || rect.Height == 0)
            {
                return null;
            }

            IntPtr hdc = Native.GetDC(hwnd);
            Bitmap bitmap = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                IntPtr dstHdc = graphics.GetHdc();
                Native.BitBlt(dstHdc, 0, 0, rect.Width, rect.Height, hdc, 0, 0, Native.RasterOperation.SRC_COPY);
                graphics.ReleaseHdc(dstHdc);
            }

            Native.ReleaseDC(hwnd, hdc);
            return bitmap;
        }

        public static void RestoreWindow(IntPtr hwnd)
        {
            if (Native.IsIconic(hwnd))
            {
                Native.ShowWindow(hwnd, Native.ShowWindowCommand.SW_RESTORE);
                Thread.Sleep(RestoreWaitTime);
            }
        }

        public static void RepositionWindow(IntPtr hwnd, int left, int top)
        {
            Rect winRect;
            Native.GetWindowRect(hwnd, out winRect);
            Native.MoveWindow(hwnd, left, top, winRect.Width, winRect.Height, true);
        }

        public static void SetForgroundWindowMethod1(Process process)
        {
            Native.AllowSetForegroundWindow(process.Id);
            Native.SetForegroundWindow(process.MainWindowHandle);
        }

        public static void SetForgroundWindowMethod2(Process process)
        {
            Native.SwitchToThisWindow(process.MainWindowHandle, true);
        }

        public static void SetForgroundWindowMethod3(Process process)
        {
            Native.keybd_event((byte)Native.KeyCodes.VK_LMENU, 0x45, Native.KeyboardFlags.KEYEVENTF_EXTENDEDKEY | 0, 0);
            Native.keybd_event((byte)Native.KeyCodes.VK_LMENU, 0x45, Native.KeyboardFlags.KEYEVENTF_EXTENDEDKEY | Native.KeyboardFlags.KEYEVENTF_KEYUP, 0);
            Native.SetForegroundWindow(process.MainWindowHandle);
        }
    }
}