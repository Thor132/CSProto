using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using AvalonDockTest.TestClasses;

namespace AvalonDockTest.AvalonDock
{
    class PanesStyleSelector : StyleSelector
    {
        public Style SimpleTabStyle
        {
            get;
            set;
        }

        public Style SimpleTab2Style
        {
            get;
            set;
        }

        public Style ChildTabStyle
        {
            get;
            set;
        }

        public Style SimpleAnchorableStyle
        {
            get;
            set;
        }

        public override System.Windows.Style SelectStyle(object item, System.Windows.DependencyObject container)
        {
            if (item is ChildTab)
                return ChildTabStyle;

            if (item is ComplexTab)
                return SimpleTab2Style;

            if (item is SimpleTab)
                return SimpleTabStyle;

            if (item is SimpleAnchorable)
                return SimpleAnchorableStyle;

            return base.SelectStyle(item, container);
        }
    }
}
