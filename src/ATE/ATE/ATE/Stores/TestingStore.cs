using ATE.Common.Mvvm;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATE.Stores
{
    public class TestingParameter : BindableBase
    {
        private string name;
        private string title;
        private string description;
        private List<object> channelResults;
        private double minValue;
        private double maxValue;

        public string Name { get { return name; } set { SetProperty(ref name, value); } }

        public string Title { get { return title; } set { SetProperty(ref title, value); } }

        public string Description { get { return description; } set { SetProperty(ref description, value); } }

        public double MinValue { get { return minValue; } set { SetProperty(ref minValue, value); } }

        public double MaxValue { get { return maxValue; } set { SetProperty(ref maxValue, value); } }

        public List<object> ChannelResults { get { return channelResults; } set { SetProperty(ref channelResults, value);} }
    }

    public class TestingStep : BindableBase
    {
        private string name;
        public string title;
        public string description;
        private bool isToTest = true;
        private bool isExpand = false;

        public string Name { get { return name; } set { SetProperty(ref name , value); } }  
        public string Title { get { return title; } set { SetProperty(ref title, value); } }
        public string Description { get { return description; } set { SetProperty(ref description, value); } }
        public bool IsToTest { get { return isToTest; } set { SetProperty(ref isToTest, value); } }
        public bool IsExpand { get { return isExpand; } set { SetProperty(ref isExpand, value); } }
        private ObservableCollection<TestingParameter> parameters;
        public ObservableCollection<TestingParameter> Parameters
        {
            get { return parameters; }
            set { SetProperty(ref parameters, value); }
        }
    }

    //public class TestingCode : BindableBase
    //{

    //    private ObservableCollection<TestingStep> steps;
    //    public ObservableCollection<TestingStep> Steps
    //    {
    //        get { return steps; }
    //        set { SetProperty(ref steps, value); }
    //    }
    //}

    public class TestingRecord : BindableBase
    {
        private ObservableCollection<TestingStep> steps;
        public ObservableCollection<TestingStep> Steps
        {
            get { return steps; }
            set { SetProperty(ref steps, value); }
        }
    }

    public class TestingStore : RegionViewModelBase, IViewModel
    {
        public TestingRecord TestingRecord { get; set; }

        public TestingStore(IRegionManager regionManager) : base(regionManager)
        {
            var a = new TestingStep()
            {
                Title = "300P调光最大",
                Parameters = new ObservableCollection<TestingParameter>{
                            new TestingParameter()
                            {
                                Name = "ouputCurrent",
                                Title = "输出电流",
                                MinValue=10,
                                MaxValue=20,
                                ChannelResults = new List <object>
                                { 15, 10, 8, 21 }
                            },
                            new TestingParameter()
                            {
                                Name = "ouputCurrent",
                                Title = "输出电压",
                                MinValue=35,
                                MaxValue=40,
                                ChannelResults = new List < object > { 37, 37, 40, 40 }
                            }

                        }
            };

            TestingRecord = new TestingRecord()
            {
                Steps = new ObservableCollection<TestingStep>()
                {
                   a, a, a, a,a ,a ,a,a ,a ,a,a,a,a,a, a,a,a,a,a,a,a,a,a,a,a,a,a,a,a,new TestingStep()
            {
                Title = "300P调光最大",
                Parameters = new ObservableCollection<TestingParameter>{ }
            }
                }
            };

            ExpandAllCommand = new DelegateCommand(ExpandAllCommandAction);
            ShrinkAllCommand = new DelegateCommand(ShrinkAllCommandAction);
            SelectAllCommand = new DelegateCommand(SelectAllCommandAction);
            UnSelectAllCommand = new DelegateCommand(UnSelectAllCommandAction);
        }

        public DelegateCommand ExpandAllCommand { get; set; }

        public DelegateCommand ShrinkAllCommand { get; set; }

        public DelegateCommand SelectAllCommand { get; set; }

        public DelegateCommand UnSelectAllCommand { get; set; }

        void ExpandAllCommandAction()
        {
            foreach (var item in TestingRecord.Steps)
            {
                item.IsExpand = true;
            }
        }


        void ShrinkAllCommandAction()
        {
            foreach (var item in TestingRecord.Steps)
            {
                item.IsExpand = false;
            }
        }

        void SelectAllCommandAction()
        {
            foreach (var item in TestingRecord.Steps)
            {
                item.IsToTest = true;
            }
        }

        void UnSelectAllCommandAction()
        {
            foreach (var item in TestingRecord.Steps)
            {
                item.IsToTest = false;
            }
        }
    }
}
