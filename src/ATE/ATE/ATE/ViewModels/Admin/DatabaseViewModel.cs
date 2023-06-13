using ATE.Common.Mvvm;
using ATE.Service;
using Prism.Mvvm;
using Prism.Ioc;
using System.Data;
using System.Collections.ObjectModel;
using ATE.Service.Entities;
using System.Collections.Generic;
using System;
using ATE.Stores;
using ATE.Services.Entities;
using System.Linq;
using System.Reflection;
using SqlSugar;
using System.Collections;

namespace ATE.ViewModels.Admin
{
    public class DatabaseViewModel : BindableBase, IViewModel
    {
        private Type currentEntityType;
        private ObservableCollection<object> dataSets;

        public DbService DbService { get; set; } = ContainerLocator.Container.Resolve<DbService>();

        public ObservableCollection<object> DataSets { get => dataSets; set { SetProperty(ref dataSets, value); } }

        public Dictionary<string, Type> DbTypes { get; set; }

        public Type CurrentEntityType { get => currentEntityType; set { currentEntityType = value; OnCurrentEntityChanged(); } }

        public DatabaseViewModel()
        {
            DataSets = new ObservableCollection<object>(DbService.Query<UserEntity>());

            DbTypes = new Dictionary<string, Type> {
                { "用户数据库", typeof(UserEntity) },
                { "配置数据库", typeof(ConfigurationEntity) },
                { "测试数据库", typeof(TestingProjectEntity) },
            };
            CurrentEntityType = DbTypes.FirstOrDefault().Value;
        }

        public void OnCurrentEntityChanged()
        {
            var method = typeof(DbService).GetMethod("Query").MakeGenericMethod(CurrentEntityType);
            //var datas =(List<object>)method.Invoke(DbService, new object[] { ""});
            //IEnumerable datas = (IEnumerable)Activator.CreateInstance(typeof(List<>).MakeGenericType(CurrentEntityType));
            IEnumerable datas = (IEnumerable)method.Invoke(DbService, new object[] { "" });
            ObservableCollection<object> collection = new ObservableCollection<object> { };
            foreach (var data in datas)
            {
                collection.Add(data);
            }
            DataSets = new ObservableCollection<object>(collection);
        }
    }
}
