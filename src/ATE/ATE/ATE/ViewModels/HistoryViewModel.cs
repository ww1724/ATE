using ATE.Common.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.ViewModels
{
    public class HistoryViewModel : RegionViewModelBase, IViewModel
    {
        public HistoryViewModel(IRegionManager regionManager) : base(regionManager)
        {
        }
    }
}
