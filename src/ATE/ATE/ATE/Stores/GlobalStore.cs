
using ATE.Common.Mvvm;
using ATE.Share;
using Prism.Commands;
using Prism.Regions;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using ATE.Models;
using ATE.Service;
using ATE.Service.Entities;

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
        public IDialogService DialogService { get; set; } = ContainerLocator.Container.Resolve<IDialogService>();
        
        public DbService DbService { get; set; }  = ContainerLocator.Container.Resolve<DbService>();

        private ObservableCollection<MenuItem> menuItems;

        public ObservableCollection<MenuItem> MenuItems { get => menuItems; set => SetProperty(ref menuItems, value); }

        private MenuItem currentMenu;

        public MenuItem CurrentMenu { get { return currentMenu; } set { SetProperty(ref currentMenu, value); OnCurrentMenuChanged(value); } }

        private User currentUser;

        public User CurrentUser { get { return currentUser; } set { SetProperty(ref currentUser, value); OnCurrentUserChanged(value); } }

        public GlobalStore(IRegionManager regionManager) : base(regionManager)
        {

            MenuItems = new ObservableCollection<MenuItem>()
            {
                new MenuItem { Icon=(char)0xe83d, Text="测试中心", Path=Constants.TestBoard } ,
                new MenuItem { Icon=(char)0xe683, Text="程序编辑", Path=Constants.EditBoard } ,
                new MenuItem { Icon=(char)0xe807, Text="数据中心", Path=Constants.History } ,
                new MenuItem { Icon=(char)0xe66d, Text="控制中心", Path=Constants.Admin } ,
            };


            // 默认用户登录
            var user = DbService.Query<UserEntity>().Where(x => x.Name == "测试用户1").FirstOrDefault();
            if (user == null)
            {
                DbService.Insert(new UserEntity() {  Name= "测试用户1", Passwd="123456", Role="Primary" });
                user = DbService.Query<UserEntity>().Where(x => x.Name == "测试用户1").FirstOrDefault();
            }
            CurrentUser = new User { Name = user.Name, Role = user.Role };

            CurrentMenu = MenuItems.FirstOrDefault();
            NavigateToCommand = new DelegateCommand<string>(NavigateToCommandAction);
            ShowLoginDialogCommand = new DelegateCommand(ShowLoginDialog);
        }

        public DelegateCommand<string> NavigateToCommand { get; set; }

        public DelegateCommand ShowLoginDialogCommand { get; set; }

        public void ShowLoginDialog()
        {
            Console.Write(currentUser.Name);
            DialogService.ShowDialog(Constants.LoginDialog);
        }

        private void OnCurrentMenuChanged(MenuItem menu)
        {
            RegionManager.RequestNavigate(Constants.MainRegion, menu.Path);
        }

        private void OnCurrentUserChanged(User value)
        {

        }

        private void NavigateToCommandAction(string path)
        {
            RegionManager.RequestNavigate(Constants.MainRegion, path);
        }
    }
}
