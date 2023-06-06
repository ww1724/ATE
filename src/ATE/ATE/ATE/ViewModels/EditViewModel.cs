using ATE.Common.Mvvm;
using ATE.Service;
using ATE.Service.Interface;
using ATE.Services.Entities;
using ATE.Stores;
using ATE.Test.Contract;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Zoranof.Workflow;
using Zoranof.WorkFlow;
using Prism.Ioc;

namespace ATE.ViewModels
{
    

    public class EditViewModel : RegionViewModelBase, IViewModel
    {
        private IDbService DbService { get; set; }

        private ObservableCollection<TestingProjectEntity> testingProjects;

        public TestingStore TestingStore { get; set; } = ContainerLocator.Container.Resolve<TestingStore>();

        public ObservableCollection<TestingProjectEntity> TestingProjects { get => testingProjects; set => SetProperty(ref testingProjects, value); }

        public EditViewModel(IRegionManager regionManager, IDbService dbService) : base(regionManager)
        {
            DbService = dbService;

            TestingProjects = new ObservableCollection<TestingProjectEntity>();

            // actions
            ViewLoadedCommand = new DelegateCommand(ViewLoadedAction);

        }

        public DelegateCommand ViewLoadedCommand { get; set; }

        public async void ViewLoadedAction()
        {

            // 加载程序集中测试项
            await Task.Run(() =>
            {
                var t = new ObservableCollection<TestingProjectEntity>();
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    var types = assembly.GetTypes().Where(x => x.GetInterfaces().Contains(typeof(ITestingProject))).ToList();
                    foreach (var x in types)
                    {
                        var attr = x.GetCustomAttribute<TestingProjectAttribute>();
                        if (attr == null || attr.Group == "Template") continue;
                        var project = new TestingProjectEntity();
                        project.Title = attr.Title;
                        t.Add(project);
                    }
                }
                TestingProjects = new ObservableCollection<TestingProjectEntity>(t);
            });

            //var data =  DbService.Query<TestingProjectEntity>();
            //TestingProjects = new ObservableCollection<TestingProjectEntity>(data);
        }
    }
}
