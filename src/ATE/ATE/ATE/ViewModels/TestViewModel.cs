using ATE.Common.Mvvm;
using ATE.Stores;
using Prism.Regions;
using Prism.Ioc;
using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace ATE.ViewModels
{


    public class TestViewModel : RegionViewModelBase, IViewModel
    {
        public TestingStore TestingStore { get; set; } 

        public TestViewModel(IRegionManager regionManager) : base(regionManager)
        {
            TestingStore = ContainerLocator.Container.Resolve<TestingStore>();
        }
    }
}
