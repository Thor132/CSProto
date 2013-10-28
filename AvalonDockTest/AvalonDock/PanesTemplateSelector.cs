using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using Xceed.Wpf.AvalonDock.Layout;
using AvalonDockTest.TestClasses;

namespace AvalonDockTest.AvalonDock
{
    class PanesTemplateSelector : DataTemplateSelector
    {
        public PanesTemplateSelector()
        {
        }

        public DataTemplate SimpleTabTemplate
        {
            get;
            set;
        }

        public DataTemplate SimpleTab2Template
        {
            get;
            set;
        }

        public DataTemplate SimpleAnchorableTemplate
        {
            get;
            set;
        }

        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            var itemAsLayoutContent = item as LayoutContent;

            if (item is SimpleTab2)
                return SimpleTab2Template;

            if (item is SimpleTab)
                return SimpleTabTemplate;

            if (item is SimpleAnchorable)
                return SimpleAnchorableTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}
