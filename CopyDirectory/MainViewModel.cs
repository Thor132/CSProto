namespace VisualCopyDirectory
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;

    public class MainViewModel : INotifyPropertyChanged
    {
        private string customText;
        private string sourceDirectory;
        private string destinationDirectory;
        private string currentFilename;
        private int totalFilesCount;
        private int copiedFilesCount;
        private long totalBytesCount;
        private long copiedBytesCount;
        private int fileProgress;
        private int sizeProgress;

        public MainViewModel()
        {
            this.CopiedBytesCount = 0;
            this.CopiedFilesCount = 0;
            this.TotalBytesCount = 0;
            this.TotalFilesCount = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties

        public string CustomText
        {
            get
            {
                return this.customText;
            }

            set
            {
                this.customText = value;
                this.OnPropertyChanged("CustomText");
                this.OnPropertyChanged("CustomTextTruncated");
            }
        }

        public string CustomTextTruncated
        {
            get
            {
                return this.TruncateString(this.CustomText, 25);
            }
        }

        public string SourceDirectory
        {
            get
            {
                return this.sourceDirectory;
            }

            set
            {
                this.sourceDirectory = value;
                this.OnPropertyChanged("SourceDirectory");
                this.OnPropertyChanged("SourceDirectoryTruncated");
            }
        }

        public string SourceDirectoryTruncated
        {
            get
            {
                return this.TruncateString(this.SourceDirectory, 45);
            }
        }

        public string DestinationDirectory
        {
            get
            {
                return this.destinationDirectory;
            }

            set
            {
                this.destinationDirectory = value;
                this.OnPropertyChanged("DestinationDirectory");
                this.OnPropertyChanged("DestinationDirectoryTruncated");
            }
        }

        public string DestinationDirectoryTruncated
        {
            get
            {
                return this.TruncateString(this.DestinationDirectory, 45);
            }
        }

        public string CurrentFilename
        {
            get
            {
                return this.currentFilename;
            }

            set
            {

                this.currentFilename = value;
                this.OnPropertyChanged("CurrentFilename");
                this.OnPropertyChanged("CurrentFilenameTruncated");
            }
        }

        public string CurrentFilenameTruncated
        {
            get
            {
                return this.TruncateString(this.CurrentFilename, 45);
            }
        }

        public int TotalFilesCount
        {
            get
            {
                return this.totalFilesCount;
            }

            set
            {
                this.totalFilesCount = value;
                this.OnPropertyChanged("TotalFilesCount");
            }
        }

        public int CopiedFilesCount
        {
            get
            {
                return this.copiedFilesCount;
            }

            set
            {
                this.copiedFilesCount = value;
                this.OnPropertyChanged("CopiedFilesCount");

                if (this.TotalFilesCount > 0)
                {
                    this.FileProgress = (int)((this.CopiedFilesCount / (double)this.TotalFilesCount) * 100);
                }
                else
                {
                    this.FileProgress = 0;
                }
            }
        }

        public long TotalBytesCount
        {
            get
            {
                return this.totalBytesCount;
            }

            set
            {
                this.totalBytesCount = value;
                this.OnPropertyChanged("TotalBytesCount");
            }
        }

        public long CopiedBytesCount
        {
            get
            {
                return this.copiedBytesCount;
            }

            set
            {
                this.copiedBytesCount = value;
                this.OnPropertyChanged("CopiedBytesCount");

                if (this.TotalBytesCount > 0)
                {
                    this.SizeProgress = (int)((this.CopiedBytesCount / (double)this.TotalBytesCount) * 100);
                }
                else
                {
                    this.SizeProgress = 0;
                }
            }
        }

        public int FileProgress
        {
            get
            {
                return this.fileProgress;
            }

            set
            {
                this.fileProgress = value;
                this.OnPropertyChanged("FileProgress");
            }
        }

        public int SizeProgress
        {
            get
            {
                return this.sizeProgress;
            }

            set
            {
                this.sizeProgress = value;
                this.OnPropertyChanged("SizeProgress");
            }
        }

        #endregion

        public void Start()
        {
            CommandLineOptions options = this.GetCommandLineOptions();
            if (options == null)
            {
                return;
            }

            this.CustomText = options.CustomText;
            Task task = Task.Factory.StartNew(() => this.CopyDirectory(options));
        }

        private CommandLineOptions GetCommandLineOptions()
        {
            CommandLineOptions options = new CommandLineOptions();
            if (!CommandLine.Parser.Default.ParseArguments(Environment.GetCommandLineArgs(), options))
            {
                this.LogError("Unable to parse command line options");
                return null;
            }

            return options;
        }

        private void CopyDirectory(CommandLineOptions options)
        {
            DirectoryInfo source = new DirectoryInfo(options.SourceDirectory);
            if (!source.Exists)
            {
                LogError(string.Format("Source directory {0} does not exist", options.SourceDirectory));
                return;
            }

            DirectoryInfo destination = new DirectoryInfo(options.DestinationDirectory);
            if (!destination.Exists)
            {
                try
                {
                    destination.Create();
                }
                catch (Exception ex)
                {
                    LogError(string.Format("An exception occurred when attempting to create destination directory: {0}", ex.Message));
                    return;
                }
            }

            this.SourceDirectory = source.FullName;
            this.DestinationDirectory = destination.FullName;

            List<FileInfo> files = source.EnumerateFiles("*", SearchOption.AllDirectories).ToList();
            this.TotalBytesCount = files.Sum(p => p.Length);
            this.TotalFilesCount = files.Count;

            foreach (FileInfo file in files)
            {
                try
                {
                    string sourceSubDir = file.DirectoryName.Replace(source.FullName, string.Empty);
                    string subDirFilename = string.Format("{0}\\{1}", sourceSubDir, file.Name);
                    this.CurrentFilename = subDirFilename.StartsWith("\\") ? subDirFilename.Remove(0, 1) : subDirFilename;

                    FileInfo destFile = new FileInfo(string.Format("{0}\\{1}", destination.FullName, this.CurrentFilename));
                    if (!destFile.Directory.Exists)
                    {
                        destFile.Directory.Create();
                    }

                    Console.WriteLine("Copying {0} to {1}", file.FullName, destFile.FullName);
                    file.CopyTo(destFile.FullName, true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An exception was encountered: {0}", ex.Message);
                }

                this.CopiedBytesCount += file.Length;
                this.CopiedFilesCount++;
            }

            this.CurrentFilename = "Finished";
            if (!options.KeepOpen)
            {
                Application.Current.Dispatcher.Invoke(new Action(() => Application.Current.Shutdown()));
            }
        }

        private void LogError(string text)
        {
            Console.WriteLine(text);
            Application.Current.Dispatcher.Invoke(new Action(() => Application.Current.Shutdown(1)));
        }

        private void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private string TruncateString(string input, int max)
        {
            if (input.Length < max)
                return input;

            max -= 3;
            return string.Format("{0}...{1}", input.Substring(0, max / 2), input.Substring((input.Length - (max / 2))));
        }
    }
}