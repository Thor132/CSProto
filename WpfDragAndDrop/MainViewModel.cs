// -----------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="N/A">
// (C) Travis Thornton 2013
// </copyright>
// -----------------------------------------------------------------------

namespace WpfDragAndDrop
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Windows;
    using GongSolutions.Wpf.DragDrop;
    using System.Collections.Generic;
    using System.Collections.Specialized;

    public class MainViewModel : IDropTarget
    {
        public MainViewModel()
        {
            this.List = new ObservableCollection<ITreeItem>();
            this.SetupDemoList();
        }

        public ObservableCollection<ITreeItem> List { get; set; }

        #region Drag and Drop

        public void DragOver(IDropInfo dropInfo)
        {
            if (this.IsFileDropInfo(dropInfo))
            {
                dropInfo.Effects = DragDropEffects.Link;
                return;
            }

            ITreeItem source = dropInfo.Data as ITreeItem;
            ITreeItem target = dropInfo.TargetItem as ITreeItem;
            if (source == null
                || target == null
                || source == target)
            {
                return;
            }

            // Cannot drag if the source's parent is read only
            if (source.Parent != null && (source.Parent.IsReadOnly || !source.IsEnabled))
            {
                return;
            }

            // Ensure the source isn't a parent to the target
            ITreeItem node = target.Parent;
            while (node != null)
            {
                if (node == source)
                {
                    return;
                }

                node = node.Parent;
            }

            // Determine the action to perform
            switch (dropInfo.InsertPosition)
            {
                // Handle dropping directly onto a node
                case RelativeInsertPosition.TargetItemCenter:
                    if (!target.IsReadOnly && target.IsEnabled)
                    {
                        dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                        dropInfo.Effects = DragDropEffects.Move;
                    }

                    break;

                // Insertion cases
                default:
                    if (target.Parent == null || (!target.Parent.IsReadOnly && target.IsEnabled))
                    {
                        dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                        dropInfo.Effects = DragDropEffects.Move;
                    }
                    break;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            if (this.HandleFileDropInfo(dropInfo))
            {
                return;
            }

            int targetIndex = dropInfo.InsertIndex;
            int sourceIndex = dropInfo.DragInfo.SourceIndex;
            ITreeItem source = dropInfo.Data as ITreeItem;
            ITreeItem target = dropInfo.TargetItem as ITreeItem;
            IList targetCollection = dropInfo.TargetCollection as IList;
            IList sourceCollection = dropInfo.DragInfo.SourceCollection as IList;

            if (source == null
                || target == null
                || targetCollection == null
                || sourceCollection == null)
            {
                return;
            }

            // Handle dropping directly onto a node
            if (dropInfo.InsertPosition == RelativeInsertPosition.TargetItemCenter)
            {
                // Don't do anything if the target was the same as the current parent
                if (source.Parent == target)
                {
                    return;
                }

                sourceCollection.RemoveAt(sourceIndex);
                target.Children.Add(source);
                source.Parent = target;
                return;
            }

            // If the target index is past the source index, modify it to account for the removal before insertion.
            if (targetCollection == sourceCollection && sourceIndex < targetIndex)
            {
                targetIndex--;
            }

            sourceCollection.RemoveAt(sourceIndex);
            targetCollection.Insert(targetIndex, source);
            source.Parent = target.Parent;
        }

        public bool IsFileDropInfo(IDropInfo dropInfo)
        {
            DataObject data = dropInfo.Data as DataObject;
            return data != null && data.GetDataPresent(DataFormats.FileDrop);
        }

        public bool HandleFileDropInfo(IDropInfo dropInfo)
        {
            if (this.IsFileDropInfo(dropInfo))
            {
                StringCollection filenames = ((DataObject)dropInfo.Data).GetFileDropList();
                foreach (string filename in filenames)
                {
                    MessageBox.Show(string.Format("File drop: {0}", filename));
                }

                return true;
            }

            return false;
        }

        #endregion

        #region Setup

        private void SetupDemoList()
        {
            const int rootCount = 5;
            const int childrenCount = 3;
            const bool firstReadOnly = true;
            int totalCount = 0;

            for (int i = 0; i < rootCount; ++i, ++totalCount)
            {
                TreeViewNode root = GenerateTreeNode("Root", i, firstReadOnly && i == 0);
                this.List.Add(root);

                for (int u = 0; u < childrenCount; ++u, ++totalCount)
                {
                    TreeViewNode child = GenerateTreeNode("Child", totalCount, firstReadOnly && u == 0);
                    child.Parent = root;
                    root.Children.Add(child);
                }
            }
        }

        private TreeViewNode GenerateTreeNode(string text, int index, bool readOnly)
        {
            TreeViewNode node = new TreeViewNode();
            node.IsEnabled = true;
            node.IsExpanded = true;
            node.IsReadOnly = readOnly;
            node.IsSelected = false;
            node.Text = string.Format("{0}{1}", index, text);
            return node;
        }

        #endregion
    }
}
