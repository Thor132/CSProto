using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ListTreeView
{
    class TreeViewNode2 : TreeViewNode
    {
        public TreeViewNode2(TreeViewNode parent)
            : base(parent)
        {
        }

        public TreeViewNode2(TreeViewNode parent, int customNumber)
            : base(parent, customNumber)
        {
        }

        public override bool IsEnabled
        {
            get
            {
                return this.Children.Any();
            }
        }
    }
}
