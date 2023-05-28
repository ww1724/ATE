using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Zoranof.UI.Wpf.Helpers
{
    public class IconHelper
    {

        public static char GetIcon(DependencyObject obj)
        {
            return (char)obj.GetValue(IconProperty);
        }

        public static void SetIcon(DependencyObject obj, char value)
        {
            obj.SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.RegisterAttached("Icon", typeof(char), typeof(IconHelper), new PropertyMetadata(null));



        public static int GetIconSize(DependencyObject obj)
        {
            return (int)obj.GetValue(IconSizeProperty);
        }

        public static void SetIconSize(DependencyObject obj, int value)
        {
            obj.SetValue(IconSizeProperty, value);
        }

        // Using a DependencyProperty as the backing store for IconSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.RegisterAttached("IconSize", typeof(int), typeof(IconHelper), new PropertyMetadata(16));


    }
}
