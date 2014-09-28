using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Threading.Tasks;

namespace UnhandledExceptions
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            AppDomain.CurrentDomain.UnhandledException += App.UnhandledTest;
            Application.Current.DispatcherUnhandledException += App.UnhandledTest2;
            InitializeComponent();

            //Task newt = new Task(() =>
            //{
            //    throw new Exception("test");
            //    Debug.WriteLine("BLAH");
            //});
            //newt.Start();
        }

        private void test(object sender, RoutedEventArgs e)
        {
            // throw new Exception("test");
            MessageBox.Show("TEST");

        }
    }
}
