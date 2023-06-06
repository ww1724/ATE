using ATE.Models;
using ATE.Stores;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using Prism.Ioc;
using Prism.Commands;
using ATE.Service;
using ATE.Service.Interface;
using ATE.Service.Entities;
using System.Linq;

namespace ATE.ViewModels.Dialog
{
    public class LoginViewModel : BindableBase, IDialogAware
    {
        public GlobalStore GlobalStore { get; set; } 

        public IDbService DbService { get; set; } = ContainerLocator.Container.Resolve<IDbService>();

        public LoginUser User { get; set; }

        public string Title => "切换用户";

        private string info = "";

        public string Info { get { return info; } set { 
                SetProperty(ref info, value); } }

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed() { }

        public void OnDialogOpened(IDialogParameters parameters) { }

        public LoginViewModel()
        {
            GlobalStore = ContainerLocator.Container.Resolve<GlobalStore>();

            //var user = GlobalStore.CurrentUser ?? new User();
            User = new LoginUser()
            {
                Name = "测试用户1"
            };

            LoginCommand = new DelegateCommand(Login);
            RegisterCommand = new DelegateCommand(Register);   
        }

        public DelegateCommand LoginCommand { get; set; }

        public DelegateCommand RegisterCommand { get; set; }

        public void Login()
        {
             var users = DbService.Query<UserEntity>();
            var user = users.Where(x => x.Name == User.Name).FirstOrDefault();
            if (user == null)
            {
                Info = "用户不存在, 请检查用户名!";
                return;
            }
            if (user.Passwd != User.Password)
            {
                Info = "密码错误, 请检查密码!";
                return;
            }
            Info = "登录成功, 正在跳转";
            GlobalStore.CurrentUser = new User()
            {
                Name = user.Name,
                Role = user.Role
            };
            RequestClose.Invoke(new DialogResult());
        }

        public void Register()
        {

            if (User.Password == null)
            {
                Info = "请输入密码";
                return;
            }

            var user = DbService.Query<UserEntity>().Where(x => x.Name == User.Name).FirstOrDefault();
            if (user != null)
            {
                Info = "用户名已存在, 请重新输入";
                return;
            }

            DbService.Insert(new UserEntity
            {
                 Name = User.Name,
                 Passwd = User.Password,
                 Role = "Primary"
            });
            user = DbService.Query<UserEntity>().Where(x => x.Name == User.Name).FirstOrDefault();
            if (user != null)
            {
                GlobalStore.CurrentUser = new User()
                {
                    Name = user.Name,
                    Role = user.Role
                };
                Info = "登录成功, 正在跳转";
                RequestClose.Invoke(new DialogResult());
            } else
            {
                Info = "注册失败, 请检查数据库";
            }

            
        }
    }
}
