using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvalonDockTest.TestClasses
{
    public class SimpleTab : ITabItem
    {
        private Uri tabIconUri = new Uri("pack://application:,,,/AvalonDockTest;component/Resources/document.png", UriKind.RelativeOrAbsolute);
        public SimpleTab()
        {
            this.Title = "SimpleTab";
            this.ContentId = "SimpleTabContentId";
            this.IsSelected = false;
            this.IsActive = false;

            this.Data = "TabData";
        }

        public string Title { get; set; }
        public string ContentId { get; set; }

        public Uri IconSource
        {
            get
            {
                return this.tabIconUri;
            }
        }

        public string Data { get; set; }
        public bool IsSelected { get; set; }
        public bool IsActive { get; set; }
    }

    public class SimpleTab2 : SimpleTab
    {
        public SimpleTab2()
        {
            this.Title = "SimpleTab TWO";
            this.Data = "SimpleTab2Data";
        }
    }
}
