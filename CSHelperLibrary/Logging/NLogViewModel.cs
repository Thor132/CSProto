// -----------------------------------------------------------------------
// <copyright file="NLogViewModel.cs" company="N/A">
// (C) Travis Thornton 2013
// </copyright>
// -----------------------------------------------------------------------

namespace CSHelperLibrary.Logging
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows.Data;
    using CSHelperLibrary.WPF;
    using NLog;

    /// <summary>
    /// NLog simple view model for use with WPF controls.
    /// </summary>
    public class NLogViewModel : NotifyPropertyChangedBase
    {
        /// <summary>
        /// The search string backing member.
        /// </summary>
        private string searchString;

        /// <summary>
        /// Initializes a new instance of the <see cref="NLogViewModel"/> class.
        /// This uses the <see cref="NLogCustomTarget"/> class' singleton log collection as the collection source.
        /// </summary>
        public NLogViewModel()
        {
            this.LogCollection = CollectionViewSource.GetDefaultView(NLogCustomTarget.Instance.LogEvents);
            this.LogCollection.Filter = new Predicate<object>(this.FilterCallback);
            this.SearchCompareOptions = CompareOptions.IgnoreCase;
        }

        /// <summary>
        /// Gets or sets the log collection view.
        /// </summary>
        public ICollectionView LogCollection { get; set; }

        /// <summary>
        /// Gets or sets the log collection filter's search string.
        /// Updating this will refresh the collection using the standard filter.
        /// </summary>
        public string SearchString
        {
            get
            {
                return this.searchString;
            }

            set
            {
                this.searchString = value;
                this.NotifyPropertyChanged("SearchString");
                this.LogCollection.Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the compare options for searching the log event collection.
        /// </summary>
        public CompareOptions SearchCompareOptions { get; set; }

        /// <summary>
        /// Filter method for the log collection.
        /// </summary>
        /// <param name="item">Object to test.</param>
        /// <returns>Whether the item should be displayed.</returns>
        private bool FilterCallback(object item)
        {
            LogEventInfo log = item as LogEventInfo;
            if (log != null)
            {
                if (!string.IsNullOrEmpty(this.SearchString))
                {
                    // Testing the log's formatted message to see if it contains the search string using the set compare options.
                    if (CultureInfo.CurrentCulture.CompareInfo.IndexOf(log.FormattedMessage, this.SearchString, this.SearchCompareOptions) >= 0)
                    {
                        return true;
                    }

                    return false;
                }

                return true;
            }

            return false;
        }
    }
}