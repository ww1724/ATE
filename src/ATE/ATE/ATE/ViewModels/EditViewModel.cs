using ATE.Common.Mvvm;
using ATE.Service;
using ATE.Service.Interface;
using ATE.Services.Entities;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.ViewModels
{
    public class EditViewModel : RegionViewModelBase, IViewModel
    {
        private IDbService DbService { get; set; }

        private ObservableCollection<TestingProjectEntity> testingProjects;

        public ObservableCollection<TestingProjectEntity> TestingProjects { get => testingProjects; set => SetProperty(ref testingProjects, value); }

        public EditViewModel(IRegionManager regionManager, IDbService dbService) : base(regionManager)
        {
            DbService = dbService;

            TestingProjects = new ObservableCollection<TestingProjectEntity>();

            // actions
            ViewLoadedCommand = new DelegateCommand(ViewLoadedAction);

        }

        public DelegateCommand ViewLoadedCommand { get; set; }

        public void ViewLoadedAction()
        {
            var data =  DbService.Query<TestingProjectEntity>();
            TestingProjects = new ObservableCollection<TestingProjectEntity>(data);
        }
    }
}
