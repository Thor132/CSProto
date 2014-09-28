using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using GongSolutions.Wpf.DragDrop;
using System.Windows;
using System.Collections;

namespace ListTreeView
{
    class TreeList : IDropTarget
    {
        public TreeList()
        {
            this.Items = new ObservableCollection<ITreeViewNode>();
        }

        public ObservableCollection<ITreeViewNode> Items { get; set; }

        public void AddChild(ITreeViewNode item)
        {
            this.RegisterNodes(item);

            // TODO: new list type that contains a method for this?
            lock (this.Items)
            {
                this.Items.Add(item);
            }
        }

        public void AddChildAtLocation(ITreeViewNode item, int index)
        {
            // TODO: new list type that contains a method for this?
            lock (this.Items)
            {
                this.Items.Insert(index, item);
            }
        }

        private void RegisterNodes(ITreeViewNode node)
        {
            node.ExpansionStateUpdated += this.OnStateUpdated;
            node.ChildAdded += this.OnNodeChildAdded;
            node.LocationUpdate += this.OnNodeLocationUpdated;

            foreach (ITreeViewNode child in node.Children)
            {
                this.RegisterNodes(child);
            }
        }

        #region DragDrop

        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            ITreeViewNode source = dropInfo.Data as ITreeViewNode;
            ITreeViewNode target = dropInfo.TargetItem as ITreeViewNode;
            if (source == null || target == null)
            {
                return;
            }

            if (target.Parent != null && (target.Parent.IsReadOnly || !target.IsEnabled))
            {
                return;
            }

            //if (target.IsEnabled && !target.IsReadOnly && target.IsExpanded && target.IsExpandable)
            //{
            //    dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            //    dropInfo.Effects = DragDropEffects.Move;
            //    return;
            //}

            dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
            dropInfo.Effects = DragDropEffects.Move;
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            ITreeViewNode source = dropInfo.Data as ITreeViewNode;
            ITreeViewNode target = dropInfo.TargetItem as ITreeViewNode;
            if (source == null || target == null)
            {
                return;
            }

            var pos = dropInfo.InsertPosition;
            int targetIndex = dropInfo.InsertIndex;
            int sourceIndex = dropInfo.DragInfo.SourceIndex;
            var coll = (IList)dropInfo.TargetCollection;
            var src = (IList)dropInfo.DragInfo.SourceCollection;
           

            //this.Items.Move(sourceIndex, targetIndex);

            //targetIndex++;

            int master = targetIndex;
            switch (dropInfo.InsertPosition)
            {
                //case RelativeInsertPosition.BeforeTargetItem | RelativeInsertPosition.TargetItemCenter:
                //    break;

                case (RelativeInsertPosition.TargetItemCenter | RelativeInsertPosition.AfterTargetItem):
                    break;

                case RelativeInsertPosition.TargetItemCenter:
                    break;

                case RelativeInsertPosition.BeforeTargetItem:
                   // master--;
                    break;

                case RelativeInsertPosition.AfterTargetItem:
                    break;
            }

            if (coll == src && sourceIndex < targetIndex)
            {
                master--;
            }

            src.RemoveAt(sourceIndex);
            coll.Insert(master, source);

            return;
            bool parented = false;
            ITreeViewNode nextNode1 = this.Items.ElementAt(master);
            if (nextNode1.IsExpanded)
            {
                if (nextNode1.IsEnabled && !nextNode1.IsReadOnly)
                {
                    parented = true;
                }
            }

            source.UpdateTest = string.Format("|{0} target: {2}({5}) master: {3}({7}) parented: {6}|", dropInfo.InsertPosition, sourceIndex, targetIndex, master, source.BasicStringText, target.BasicStringText, parented, nextNode1.BasicStringText);


            ITreeViewNode nextNode = this.Items.ElementAt(master);
            if (nextNode.IsExpanded)
            {
                if (!nextNode.IsEnabled && nextNode.IsReadOnly)
                {
                    return;
                }

                source.UpdateParent(nextNode);
            }
            else
            {
                source.UpdateParent(nextNode.Parent);
            }

            source.UpdateLocation();
        }

        #endregion

        #region Tree functionality

        private void ExpandNode(ITreeViewNode item)
        {
            if (item.Children.Any())
            {
                int location = Items.IndexOf(item);

                // TODO: new list type that contains a method for this?
                lock (this.Items)
                {
                    foreach (ITreeViewNode child in item.Children)
                    {
                        this.Items.Insert(++location, child);
                    }
                }
            }
        }

        private void CollapseNode(ITreeViewNode item)
        {
            if (item.Children.Any())
            {
                // TODO: new list type that contains a method for this?
                lock (this.Items)
                {
                    foreach (ITreeViewNode child in item.Children)
                    {
                        // TODO: look into a more efficient implementation
                        this.Items.Remove(child);
                    }
                }
            }
        }

        #endregion

        #region Event Handlers

        private void OnNodeChildAdded(object sender, TreeNodeEventArgs args)
        {
            args.Node.ExpansionStateUpdated += this.OnStateUpdated;
            args.Node.ChildAdded += this.OnNodeChildAdded;
        }

        private void OnStateUpdated(object sender, EventArgs args)
        {
            ITreeViewNode node = sender as ITreeViewNode;
            if (node.IsExpanded)
            {
                this.ExpandNode(node);
            }
            else
            {
                this.CollapseNode(node);
            }
        }

        private void OnNodeLocationUpdated(object sender, EventArgs args)
        {
            ITreeViewNode node = sender as ITreeViewNode;
            if (node.IsExpanded)
            {
                this.CollapseNode(node);
                this.ExpandNode(node);
            }
        }

        #endregion
    }
}
