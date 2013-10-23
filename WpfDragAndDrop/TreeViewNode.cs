// -----------------------------------------------------------------------
// <copyright file="TreeViewNode.cs" company="N/A">
// (C) Travis Thornton 2013
// </copyright>
// -----------------------------------------------------------------------

namespace WpfDragAndDrop
{
    using System.Collections.ObjectModel;

    class TreeViewNode : ITreeItem
    {
        public TreeViewNode()
        {
            this.Children = new ObservableCollection<ITreeItem>();
        }

        public bool IsExpanded { get; set; }
        public bool IsSelected { get; set; }
        public ITreeItem Parent { get; set; }

        public ObservableCollection<ITreeItem> Children { get; private set; }

        // These should be private set, but allowing public set to put setup logic in the main view model and only use these as containers.
        public string Text { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsReadOnly { get; set; }
    }
}
