using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Diagnostics;
using System.Windows.Threading;

namespace UnhandledExceptions
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
        }

        public static void UnhandledTest(object sender, UnhandledExceptionEventArgs e)
        {
            Debug.WriteLine("unhandled caught");
        }

        public static void UnhandledTest2(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Debug.WriteLine("unhandled caught2");
    }
    }
}
