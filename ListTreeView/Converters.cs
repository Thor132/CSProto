using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Globalization;

namespace ListTreeView
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return Visibility.Visible;
            }

            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type type, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class IndentMarginConverter : IValueConverter
    {
        private const double IndentSize = 10.0;

        public object Convert(object value, Type type, object parameter, CultureInfo culture)
        {
            return new Thickness((uint)value * IndentSize, 2, 0, 0);
        }

        public object ConvertBack(object value, Type type, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}