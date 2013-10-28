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
using Xceed.Wpf.AvalonDock.Layout.Serialization;
using System.IO;

namespace AvalonDockTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.DataContext = new MainViewModel();
            InitializeComponent();

            this.DockingManager.Loaded += new RoutedEventHandler(DockingManager_Loaded);
        }

        void DockingManager_Loaded(object sender, RoutedEventArgs e)
        {
            if (new FileInfo(@".\AvalonDock.layout").Exists)
            {
                var layoutSerializer = new XmlLayoutSerializer(this.DockingManager);
                layoutSerializer.Deserialize(@".\AvalonDock.layout");
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            var layoutSerializer = new XmlLayoutSerializer(this.DockingManager);
            layoutSerializer.Serialize(@".\AvalonDock.layout");

            base.OnClosing(e);
        }
    }
}
