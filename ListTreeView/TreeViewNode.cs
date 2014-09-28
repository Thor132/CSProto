using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ListTreeView
{
    public class TreeNodeEventArgs : EventArgs
    {
        public TreeNodeEventArgs(ITreeViewNode node)
        {
            this.Node = node;
        }

        public ITreeViewNode Node { get; set; }
    }

    class TreeViewNode : INotifyPropertyChanged, ITreeViewNode
    {
        static int num;

        public bool isExpanded;
        private bool isSelected;
        private string baseText;
        public TreeViewNode(ITreeViewNode parent)
        {
            this.Text = string.Format("ID{0}", num++);
            baseText = this.Text;
            this.Children = new ObservableCollection<ITreeViewNode>();
            this.Parent = parent;
            this.IsExpanded = true;
        }
        static bool readonlyy = true;
        bool writable;
        public TreeViewNode(ITreeViewNode parent, int customNumber)
        {
            this.Text = string.Format("{0} - ID{1}", parent != null ? parent.Text : "Root", num++);
            baseText = this.Text;
            this.Children = new ObservableCollection<ITreeViewNode>();
            this.Parent = parent;
            this.IsExpanded = true;
            writable = readonlyy;
            readonlyy = false;

            
            for (int i = 0; i < customNumber; ++i)
            {
                if (this.writable)
                {
                    this.AddChild(new TreeViewNode(this, 1));
                }

                this.AddChild(new TreeViewNode(this));
                // these are not aware of the tree to get the event hookup
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler LocationUpdate;
        public event EventHandler ExpansionStateUpdated;
        public event EventHandler<TreeNodeEventArgs> ChildAdded;

        public void AddChild(ITreeViewNode child)
        {
            this.Children.Add(child);
            if (this.ChildAdded != null)
            {
                this.ChildAdded(this, new TreeNodeEventArgs(child));
            }
        }

        #region ITreeViewNode

        public ITreeViewNode Parent { get; set; }
        public ObservableCollection<ITreeViewNode> Children { get; set; }

        public string Text { get; set; }
        public uint HierarchyLevel { get { return this.GetHierarchyLevel(); } }

        public virtual bool IsEnabled { get { return true; } }
        public bool IsExpanded
        {
            get
            {
                return this.isExpanded;
            }

            set
            {
                if (value != this.isExpanded)
                {
                    this.isExpanded = value;
                    this.OnPropertyChanged("IsExpanded");
                    this.UpdateExpansion();

                }
            }
        }

        public bool IsExpandable { get { return this.Children.Any(); } }
        public bool IsReadOnly { get { return this.writable; } }

        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }

            set
            {
                this.isSelected = value;
                this.OnPropertyChanged("IsSelected");
            }
        }

        public bool IsSelectable { get { return true; } }

        public void UpdateParent(ITreeViewNode parent)
        {

            this.Text = baseText + " " + this.UpdateTest + " Updating to parent " + (parent == null ? "null" : parent.Text);
            this.Parent = parent;
            this.OnPropertyChanged("HierarchyLevel");
        }

        public void UpdateLocation()
        {
            this.LocationUpdate(this, null);
        }

        public string BasicStringText { get { return this.baseText; } }
        public string UpdateTest { get; set; }
        #endregion

        private void UpdateExpansion()
        {
            if (this.ExpansionStateUpdated != null)
            {
                this.ExpansionStateUpdated(this, null);
            }
        }

        private uint GetHierarchyLevel()
        {
            uint level = 0;
            ITreeViewNode node = this.Parent;
            while (node != null)
            {
                ++level;
                node = node.Parent;
            }

            return level;
        }

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
