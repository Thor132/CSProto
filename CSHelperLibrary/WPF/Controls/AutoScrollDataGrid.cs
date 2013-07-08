// -----------------------------------------------------------------------
// <copyright file="AutoScrollDataGrid.cs" company="N/A">
// (C) Travis Thornton 2013
// </copyright>
// -----------------------------------------------------------------------

namespace CSHelperLibrary.WPF.Controls
{
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Auto-scrolling WPF data grid
    /// </summary>
    public class AutoScrollDataGrid : DataGrid
    {
        /// <summary>
        /// Stores the ScrollViewer instance of the DataGrid.
        /// </summary>
        private ScrollViewer scrollViewer;

        /// <summary>
        /// Stores the last added object to the DataGrid.
        /// </summary>
        private object lastAddedObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoScrollDataGrid"/> class.
        /// </summary>
        public AutoScrollDataGrid()
        {
            // Sets this control's style based on the DataGrid style.
            this.Style = new Style(typeof(AutoScrollDataGrid), (Style)this.TryFindResource(typeof(DataGrid)));
        }

        /// <summary>
        /// Override the OnItemsChanged method in order to scroll to the last inserted item.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            if (this.scrollViewer == null)
            {
                this.scrollViewer = this.GetVisualChild<ScrollViewer>(this);

                if (this.scrollViewer == null)
                {
                    return;
                }
            }

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null && e.NewItems.Count > 0)
                    {
                        this.lastAddedObject = e.NewItems[e.NewItems.Count - 1];
                        if (this.scrollViewer.VerticalOffset == this.scrollViewer.ScrollableHeight)
                        {
                            this.scrollViewer.ScrollToBottom();
                            return;
                        }

                        if (this.scrollViewer.VerticalOffset == 0.0f)
                        {
                            this.scrollViewer.ScrollToTop();
                            return;
                        }

                        this.ScrollIntoView(this.lastAddedObject);
                    }

                    break;

                case NotifyCollectionChangedAction.Reset:
                    if (this.SelectedItem != null)
                    {
                        this.ScrollIntoView(this.SelectedItem);
                        return;
                    }

                    if (this.lastAddedObject != null)
                    {
                        this.ScrollIntoView(this.lastAddedObject);
                        return;
                    }

                    break;
            }
        }

        /// <summary>
        /// Traverses the visual tree for the passed in element in order to return the requested type.
        /// </summary>
        /// <typeparam name="T">Type to return.</typeparam>
        /// <param name="control">Control to traverse the tree.</param>
        /// <returns>An instance of the requested type found in the tree or null.</returns>
        private T GetVisualChild<T>(DependencyObject control) where T : Visual
        {
            T child = default(T);
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(control); i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(control, i);
                child = v as T;
                if (child == null)
                {
                    child = this.GetVisualChild<T>(v);
                }

                if (child != null)
                {
                    break;
                }
            }

            return child;
        }
    }
}