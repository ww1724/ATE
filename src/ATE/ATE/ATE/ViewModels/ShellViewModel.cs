using ATE.Common.Mvvm;
using ATE.Stores;
using Prism.Mvvm;
using Prism.Ioc;
using Prism.Regions;
using ATE.Service.Interface;

namespace ATE.ViewModels
{
    public class ShellViewModel : RegionViewModelBase, IViewModel
    {
        public GlobalStore GlobalStore { get; set; }
           
        public IDbService DbService { get; set; }

        public ShellViewModel(IRegionManager regionManager, IDbService dbService) : base(regionManager)
        {
            GlobalStore = ContainerLocator.Container.Resolve<GlobalStore>();
            
        }

       


    }
}
