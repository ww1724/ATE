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

namespace ATE.ViewModels.Admin
{
    public class DatabaseViewModel : BindableBase, IViewModel
    {
        private Type currentEntityType;

        public DbService DbService { get; set; } = ContainerLocator.Container.Resolve<DbService>();

        public DataSet UniversalDataSet { get; set; }

        public ObservableCollection<object> DataSets { get; set; }

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
            //var type = typeof(DbService);
            //MethodInfo methodInfo = type.GetMethod("Query").MakeGenericMethod();
            //DataSets = new ObservableCollection<object>(methodInfo.Invoke(DbService, new object[] {}));
        }
    }
}
