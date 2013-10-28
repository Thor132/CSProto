using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvalonDockTest.TestClasses
{
    class SimpleAnchorable : IPanelItem
    {
        private Uri tabIconUri = new Uri("pack://application:,,,/AvalonDockTest;component/Resources/document.png", UriKind.RelativeOrAbsolute);
        public SimpleAnchorable()
        {
            this.Title = "SimpleAnchorable";
            this.ContentId = "SimpleAnchorableContentId";
            this.IsSelected = false;
            this.IsActive = false;

            this.Data = "SimpleAnchorableData";
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
}
