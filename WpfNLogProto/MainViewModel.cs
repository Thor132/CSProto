// -----------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace WpfNLogProto
{
    using System.Windows.Input;
    using CSHelperLibrary.Logging;
    using CSHelperLibrary.WPF;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MainViewModel
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static int testa = 0;

        public MainViewModel()
        {
            logger.Debug("Loading");
            NLogCustomTarget.RegisterCustomTarget(NLog.LogLevel.Trace);
        }

        public ICommand Command1
        {
            get
            {
                return new RelayCommand(param =>
                {
                    this.LogSomething();
                }, param => { return true; });
            }
        }
        public void LogSomething()
        {
            ++testa;
            logger.Trace(string.Format("Something {0}", testa));
            logger.Debug(string.Format("Something {0}", testa));
            logger.Info(string.Format("Something {0}", testa));
            logger.Warn(string.Format("Something {0}", testa));
            logger.Error(string.Format("Something {0}", testa));
            logger.Fatal(string.Format("Something {0}", testa));
        }
    }
}