
using ATE.Common.Mvvm;
using ATE.Share;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ATE.Stores
{
    public class MenuItem
    {
        public Char Icon { get; set; }

        public string Text { get; set; }

        public string Path { get; set; }
    }

    public class GlobalStore : RegionViewModelBase, IViewModel
    {
        #region private
        private ObservableCollection<MenuItem> menuItems;
        private MenuItem currentMenu;
        #endregion


        public ObservableCollection<MenuItem> MenuItems { get => menuItems; set => SetProperty(ref menuItems, value); }

        public MenuItem CurrentMenu { get { return currentMenu; } set { SetProperty(ref currentMenu, value); OnCurrentMenuChanged(value); } }

        public GlobalStore(IRegionManager regionManager) : base(regionManager)
        {

            MenuItems = new ObservableCollection<MenuItem>()
            {
                new MenuItem { Icon=(char)0xe83d, Text="测试中心", Path=Constants.TestBoard } ,
                new MenuItem { Icon=(char)0xe683, Text="程序编辑", Path=Constants.EditBoard } ,
                new MenuItem { Icon=(char)0xe807, Text="数据中心", Path=Constants.History } ,
                new MenuItem { Icon=(char)0xe66d, Text="控制中心", Path=Constants.Admin } ,
            };

            CurrentMenu = MenuItems.FirstOrDefault();
            NavigateToCommand = new DelegateCommand<string>(NavigateToCommandAction);
        }

        public DelegateCommand<string> NavigateToCommand { get; set; }


        private void OnCurrentMenuChanged(MenuItem menu)
        {
            RegionManager.RequestNavigate(Constants.MainRegion, menu.Path);
        }

        private void NavigateToCommandAction(string path)
        {
            RegionManager.RequestNavigate(Constants.MainRegion, path);
        }
    }
}
