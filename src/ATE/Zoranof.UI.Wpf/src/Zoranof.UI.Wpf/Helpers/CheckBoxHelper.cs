using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Zoranof.UI.Wpf.Commons;

namespace Zoranof.UI.Wpf.Helpers
{
    public class CheckBoxHelper
    {




        public static CheckBoxSize GetSize(DependencyObject obj)
        {
            return (CheckBoxSize)obj.GetValue(SizeProperty);
        }

        public static void SetSize(DependencyObject obj, CheckBoxSize value)
        {
            obj.SetValue(SizeProperty, value);
        }

        // Using a DependencyProperty as the backing store for Size.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.RegisterAttached("Size", typeof(CheckBoxSize), typeof(CheckBoxHelper), new PropertyMetadata(CheckBoxSize.Medium));




    }
}
