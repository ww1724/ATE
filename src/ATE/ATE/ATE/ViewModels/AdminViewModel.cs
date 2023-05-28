using ATE.Common.Mvvm;
using ATE.Stores;
using Prism.Regions;
using Prism.Ioc;


namespace ATE.ViewModels
{
    public class AdminViewModel : RegionViewModelBase, IViewModel
    {
        public GlobalStore GlobalStore { get; set; }

        public AdminViewModel(IRegionManager regionManager) : base(regionManager)
        {
            GlobalStore = ContainerLocator.Container.Resolve<GlobalStore>();
        }
    }
}
