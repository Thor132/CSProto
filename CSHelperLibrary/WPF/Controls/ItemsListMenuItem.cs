// -----------------------------------------------------------------------
// <copyright file="ItemsListMenuItem.cs" company="N/A">
// (C) Travis Thornton 2013
// </copyright>
// -----------------------------------------------------------------------

namespace CSHelperLibrary.WPF.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// A MenuItem that inserts controlled MenuItems based on a bound string collection.
    /// </summary>
    public class ItemsListMenuItem : MenuItem
    {
        private MenuItem parentMenu;
        private MenuItem subMenu;
        private List<MenuItem> createdMenuItems = new List<MenuItem>();

        public static readonly DependencyProperty ItemListProperty = DependencyProperty.Register("ItemList", typeof(ObservableCollection<string>), typeof(ItemsListMenuItem));
        public static readonly DependencyProperty ItemCommandProperty = DependencyProperty.Register("ItemCommand", typeof(ICommand), typeof(ItemsListMenuItem));

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsListMenuItem"/> class.
        /// </summary>
        public ItemsListMenuItem()
        {
            this.Loaded += this.MenuItemList_Loaded;
            this.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Gets or sets the item list.
        /// </summary>
        public ObservableCollection<string> ItemList
        {
            get { return this.GetValue(ItemListProperty) as ObservableCollection<string>; }
            set { this.SetValue(ItemListProperty, value); }
        }

        /// <summary>
        /// Gets or sets the command for each item.
        /// The item string will be passed as the parameter to this command.
        /// </summary>
        public ICommand ItemCommand
        {
            get { return this.GetValue(ItemCommandProperty) as ICommand; }
            set { this.SetValue(ItemCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the submenu header.
        /// If this is not set then the items will be in the parent of this control.
        /// </summary>
        public string SubMenuHeader { get; set; }

        /// <summary>
        /// Gets or sets the maximum path length if the item list is a path.
        /// Setting this value to 0 or less than 0 will not truncate the path.
        /// </summary>
        public int MaxPathLength { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to prefix each item in the list with an index number
        /// </summary>
        public bool PrefixItemIndex { get; set; }

        #region Event Handlers

        /// <summary>
        /// Sets up this control's functionality once it is loaded.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void MenuItemList_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= this.MenuItemList_Loaded;

            MenuItem parent = this.Parent as MenuItem;
            if (parent == null)
            {
                throw new Exception("Parent must be a type of MenuItem.");
            }

            this.parentMenu = parent;
            this.ItemList.CollectionChanged += this.ItemList_CollectionChanged;

            if (!string.IsNullOrEmpty(this.SubMenuHeader))
            {
                this.subMenu = new MenuItem() { Header = this.SubMenuHeader };
                this.parentMenu.Items.Insert(this.parentMenu.Items.IndexOf(this) + 1, this.subMenu);
            }

            this.UpdateMenu();
        }

        /// <summary>
        /// Updates the menu when the item list is changed.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void ItemList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.UpdateMenu();
        }

        #endregion

        #region MenuItem Update Functionality

        /// <summary>
        /// Updates the menu.
        /// If a sub menu name was provided it will enable or disable it depending on if there are items in the list.
        /// </summary>
        private void UpdateMenu()
        {
            if (this.subMenu != null)
            {
                if (this.ItemList.Any())
                {
                    if (!this.subMenu.IsEnabled)
                    {
                        this.subMenu.IsEnabled = true;
                    }
                }
                else
                {
                    if (this.subMenu.IsEnabled)
                    {
                        this.subMenu.IsEnabled = false;
                    }
                }
            }

            this.UpdateList();
        }

        /// <summary>
        /// Updates the parent MenuItem with the items list.
        /// TODO: This is clearing and recreating the list each update. Perhaps make it reuse the already created data.
        /// </summary>
        private void UpdateList()
        {
            MenuItem parent = this.subMenu ?? this.parentMenu;
            foreach (MenuItem item in createdMenuItems)
            {
                parent.Items.Remove(item);
            }

            this.createdMenuItems.Clear();

            int nextItemIndex = this.subMenu == null ? this.parentMenu.Items.IndexOf(this) + 1 : 0;
            if (this.ItemList != null && this.ItemList.Any())
            {
                int currentIndex = nextItemIndex;
                for (int i = 0; i < this.ItemList.Count; ++i)
                {
                    MenuItem newItem = new MenuItem()
                        {
                            Header = string.Format(
                                        "{0}{1}",
                                        this.PrefixItemIndex ? string.Format("{0}: ", i + 1) : string.Empty,
                                        this.GetFormattedPath(this.ItemList[i])),
                            Command = this.ItemCommand,
                            CommandParameter = this.ItemList[i]
                        };

                    parent.Items.Insert(currentIndex++, newItem);
                    this.createdMenuItems.Add(newItem);
                }
            }
        }

        #endregion

        #region Path Truncation Functionality
        [DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
        private static extern bool PathCompactPathEx([Out] StringBuilder pszOut, string szPath, int cchMax, int dwFlags);

        /// <summary>
        /// Returns the formatted path.
        /// If MaxPathLength > 0 it will be truncated using the native PathCompactPathEx call
        /// </summary>
        /// <param name="path">Path to truncate.</param>
        /// <returns>Truncated path.</returns>
        private string GetFormattedPath(string path)
        {
            if (this.MaxPathLength <= 0)
            {
                return path;
            }

            StringBuilder sb = new StringBuilder(this.MaxPathLength + 1);
            PathCompactPathEx(sb, path, this.MaxPathLength + 1, 0);
            return sb.ToString();
        }

        #endregion
    }
}
