using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CSHelperLibrary.WPF;
using System.Collections.ObjectModel;
using System.Windows;

namespace WPFMRUMenuItemTest
{
    class MainViewModel : NotifyPropertyChangedBase
    {
        public MainViewModel()
        {
            this.MenuItems = new ObservableCollection<string>();
            for (int i = 0; i < 2; ++i)
            {
                this.MenuItems.Add(string.Format("{1}", i, DateTime.Now));
            }
        }

        public ObservableCollection<string> MenuItems { get; set; }

        public ICommand AddCommand
        {
            get
            {
                return new RelayCommand(param => this.MenuItems.Add(string.Format("{1}", this.MenuItems.Count, DateTime.Now)), param => { return true; });
            }
        }

        public ICommand RemoveCommand
        {
            get
            {
                return new RelayCommand(param => this.MenuItems.RemoveAt(0), param => this.MenuItems.Any());
            }
        }

        public ICommand RecentItemCommand
        {
            get
            {
                return new RelayCommand(param => MessageBox.Show(string.Format("RecentItemCommand param: {0}", param)));
            }
        }
    }
}
