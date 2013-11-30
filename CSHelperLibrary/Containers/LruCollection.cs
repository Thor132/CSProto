// -----------------------------------------------------------------------
// <copyright file="LruCollection.cs" company="N/A">
// (C) Travis Thornton 2013
// </copyright>
// -----------------------------------------------------------------------

namespace CSHelperLibrary.Containers
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// LRU collection class.
    /// </summary>
    /// <typeparam name="T">Collection item type.</typeparam>
    public class LruCollection<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LruCollection"/> class.
        /// </summary>
        public LruCollection()
        {
            this.List = new ObservableCollection<T>();
            this.MaxItems = 10;
        }

        /// <summary>
        /// Gets or sets the collection list.
        /// </summary>
        public ObservableCollection<T> List { get; set; }

        /// <summary>
        /// Gets or sets the max items in the list.
        /// </summary>
        public uint MaxItems { get; set; }

        /// <summary>
        /// Updates an item in the list.
        /// </summary>
        /// <param name="item">Item to update.</param>
        public void Update(T item)
        {
            int index = this.List.IndexOf(item);
            if (index > 0)
            {
                this.List.Move(index, 0);
            }
            else if (index < 0)
            {
                this.List.Insert(0, item);
            }

            this.UpdateList();
        }

        /// <summary>
        /// Updates the list to remove from the end if the count exceeds the max number set.
        /// </summary>
        public void UpdateList()
        {
            while (this.List.Count > 0 && this.List.Count > this.MaxItems)
            {
                this.List.RemoveAt(this.List.Count - 1);
            }
        }
    }
}
