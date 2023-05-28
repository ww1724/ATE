using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoranof.UI.Wpf.Themes
{
    public static class ThemeCatcher
    {
        private static Dictionary<string, string> ThemeList = new Dictionary<string, string>()
        {
            { "red", "pack://application:,,,/Zoranof.UI.Wpf;component/Themes/Red.xaml" },
            { "blue", "pack://application:,,,/Zoranof.UI.Wpf;component/Themes/Blue.xaml" },
            { "pink", "pack://application:,,,/Zoranof.UI.Wpf;component/Themes/Pink.xaml" },
            { "yellow", "pack://application:,,,/Zoranof.UI.Wpf;component/Themes/Yellow.xaml" },
            { "green", "pack://application:,,,/Zoranof.UI.Wpf;component/Themes/Green.xaml" },
            { "orange", "pack://application:,,,/Zoranof.UI.Wpf;component/Themes/Orange.xaml" },
            { "purple", "pack://application:,,,/Zoranof.UI.Wpf;component/Themes/Purple.xaml" },
        };


        public static string GetTheme(string theme)
        {
            return ThemeList[theme];
        }
    }
}
