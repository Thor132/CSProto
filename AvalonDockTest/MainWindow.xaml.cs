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
using Xceed.Wpf.AvalonDock.Layout;
using System.Collections.ObjectModel;
using System.Collections;

namespace AvalonDockTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<LayoutFloatingWindow> ShadowFloatingWindows { get; set; }
        public MainWindow()
        {
            this.DataContext = new MainViewModel();
            InitializeComponent();
            ShadowFloatingWindows = new ObservableCollection<LayoutFloatingWindow>();
            this.DockingManager.Loaded += new RoutedEventHandler(DockingManager_Loaded);
            this.DockingManager.Layout.FloatingWindows.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(FloatingWindows_CollectionChanged);
            this.AnchorablePane2.PropertyChanging += new System.ComponentModel.PropertyChangingEventHandler(AnchorablePane2_PropertyChanging);
        }

        void FloatingWindows_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<LayoutFloatingWindow> obsSender = sender as ObservableCollection<LayoutFloatingWindow>;

            int y = 0;

            // this won't work - the data was changed on collection changed.
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    this.ShadowFloatingWindows.Add(obsSender.ElementAt(e.NewStartingIndex));
                    break;

                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    var data = this.ShadowFloatingWindows.ElementAt(e.OldStartingIndex);

                    this.ShadowFloatingWindows.RemoveAt(e.OldStartingIndex);
                    break;

                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    break;
            }

            if (e.OldItems != null)
            {
                foreach (LayoutDocumentFloatingWindow win in e.OldItems)
                {
                    // Maybe use a shadow version?
                    // need to get the view and be able to operate on it (dock everything)
                    var somthing = win.RootDocument;

                    int i = 9;
                }
            }
        }

        void AnchorablePane2_PropertyChanging(object sender, System.ComponentModel.PropertyChangingEventArgs e)
        {
            int y = 0;
        }

        void DockingManager_Loaded(object sender, RoutedEventArgs e)
        {
            //if (new FileInfo(@".\AvalonDock.layout").Exists)
            //{
            //    var layoutSerializer = new XmlLayoutSerializer(this.DockingManager);
            //    layoutSerializer.Deserialize(@".\AvalonDock.layout");
            //}
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            var layoutSerializer = new XmlLayoutSerializer(this.DockingManager);
            layoutSerializer.Serialize(@".\AvalonDock.layout");

            base.OnClosing(e);
        }
    }
}
