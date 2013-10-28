using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvalonDockTest.TestClasses
{
    public class SimpleTab : ITabItem
    {
        public SimpleTab()
        {
            this.Title = "SimpleTab";
            this.ContentId = "SimpleTabContentId";
            this.Data = "TabData";
            this.IsSelected = true;
            this.IsActive = true;
        }

        public string Title { get; set; }

        public Uri IconSource { get { return new Uri("pack://application:,,,/AvalonDockTest;component/Resources/document.png", UriKind.RelativeOrAbsolute); } }

        public string ContentId { get; set; }
        public bool IsSelected { get; set; }
        public bool IsActive { get; set; }

        public string Data { get; set; }
    }

    public class SimpleTab2 : SimpleTab
    {
        public SimpleTab2()
        {
            this.Title = "SimpleTab TWO!";
            this.Data = "SimpleTab2Data";
        }
    }
}
