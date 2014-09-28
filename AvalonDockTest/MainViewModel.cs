using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;
using Xceed.Wpf.AvalonDock.Layout;
using Xceed.Wpf.AvalonDock.Controls;
using System.Windows.Media;
using AvalonDockTest.TestClasses;

namespace AvalonDockTest
{
    class MainViewModel
    {
        public MainViewModel()
        {
            this.Title = "MainViewModel";
            this.Tabs = new ObservableCollection<ITabItem>();
            this.Panels = new ObservableCollection<IPanelItem>();

            this.Panels.Add(new SimpleAnchorable());
            this.Tabs.Add(new SimpleTab());
            this.Tabs.Add(new ComplexTab());
            this.SelectedTab = this.Tabs[1];
        }

        public string Title { get; set; }
        public ObservableCollection<ITabItem> Tabs { get; set; }
        public ObservableCollection<IPanelItem> Panels { get; set; }

        public ITabItem SelectedTab { get; set; }
    }
}
