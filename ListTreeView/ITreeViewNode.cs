using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ListTreeView
{
    public interface ITreeViewNode
    {
        event EventHandler ExpansionStateUpdated;
        event EventHandler<TreeNodeEventArgs> ChildAdded;
        event EventHandler LocationUpdate;

        ITreeViewNode Parent { get; }
        ObservableCollection<ITreeViewNode> Children { get; }

        string Text { get; }
        uint HierarchyLevel { get; }

        bool IsEnabled { get; }
        bool IsExpanded { get; set; }
        bool IsExpandable { get; }
        bool IsReadOnly { get; }
        bool IsSelected { get; set; }
        bool IsSelectable { get; }

        string BasicStringText { get; }
        string UpdateTest { get; set; }
        void UpdateLocation();
        void UpdateParent(ITreeViewNode parent);
    }
}