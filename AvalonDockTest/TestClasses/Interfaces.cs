using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvalonDockTest.TestClasses
{
    interface ITabItem
    {
        string Title { get; set; }
        string ContentId { get; set; }
        Uri IconSource { get; }

        bool IsSelected { get; set; }
        bool IsActive { get; set; }

        string Data { get; set; }
    }

    interface IPanelItem
    {
        string Title { get; set; }
        string ContentId { get; set; }
        Uri IconSource { get; }

        bool IsSelected { get; set; }
        bool IsActive { get; set; }

        string Data { get; set; }
    }
}
