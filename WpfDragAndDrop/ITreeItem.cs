// -----------------------------------------------------------------------
// <copyright file="ITreeItem.cs" company="N/A">
// (C) Travis Thornton 2013
// </copyright>
// -----------------------------------------------------------------------

namespace WpfDragAndDrop
{
    using System.Collections.ObjectModel;

    public interface ITreeItem
    {
        bool IsExpanded { get; set; }
        bool IsSelected { get; set; }

        ITreeItem Parent { get; set; }
        ObservableCollection<ITreeItem> Children { get; }

        string Text { get; }
        bool IsEnabled { get; }
        bool IsReadOnly { get; }
    }
}
