using ATE.Common.Mvvm;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace ATE.Stores
{
    public class TestingResult : BindableBase
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

        public List<object> ChannelResults { get { return channelResults; } set { SetProperty(ref channelResults, value); } }
    }

    //public class TestingParameter : BindableBase
    //{
    //    public string Name { get; set; }
    //    public string Title { get; set; }
    //    public bool ParameterToAllChannel { get; set; } = true;
    //    public Type Type { get; set; }
    //    public object Value { get; set; }

    //    public List<object> ChannelValues { get; set; } = new List<object>();
    //}

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

        public List<object> ChannelResults { get { return channelResults; } set { SetProperty(ref channelResults, value); } }
    }

    [ExpandableObject]
    public class TestingStep : BindableBase
    {
        private string name;
        public string title;
        public string description;
        private bool isToTest = true;
        private bool isExpand = false;

        [Category("Information")]
        [DisplayName("First Name")]
        [Description("This property uses a TextBox as the default editor.")]
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

        private ObservableCollection<TestingResult> results;
        public ObservableCollection<TestingResult> Results
        {
            get { return results; }
            set { SetProperty(ref results, value); }
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

    [ExpandableObject]
    public class TestingRecord : BindableBase
    {
        [ExpandableObject]
        public TestingStep TestingStep { get; set; }

        private ObservableCollection<TestingStep> steps;
        public ObservableCollection<TestingStep> Steps
        {
            get { return steps; }
            set { SetProperty(ref steps, value); }
        }
    }

    public class TestingStore : RegionViewModelBase, IViewModel
    {
        [ExpandableObject]
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
                }
            };

            TestingRecord.Steps.Add(new TestingStep()
            {
                Title = "启动测试"
            });
            TestingRecord.Steps.Add(new TestingStep()
            {
                Title = "输入对输出高压测试"
            });
            TestingRecord.Steps.Add(new TestingStep()
            {
                Title = "输入对地高压测试"
            });
            TestingRecord.Steps.Add(new TestingStep()
            {
                Title = "输出对地高压测试"
            });
            TestingRecord.Steps.Add(new TestingStep()
            {
                Title = "小负载测试"
            });
            TestingRecord.Steps.Add(new TestingStep()
            {
                Title = "大负载测试"
            });
            TestingRecord.Steps.Add(new TestingStep()
            {
                Title = "短路&恢复测试"
            });
            TestingRecord.Steps.Add(new TestingStep()
            {
                Title = "300P调光测试"
            });
            TestingRecord.Steps.Add(new TestingStep()
            {
                Title = "153P调光测试"
            });
            TestingRecord.Steps.Add(new TestingStep()
            {
                Title = "0-10V调光测试"
            });
            TestingRecord.Steps.Add(new TestingStep()
            {
                Title = "变压器放电"
            });
            TestingRecord.Steps.Add(new TestingStep()
            {
                Title = "变压器放电"
            });
            TestingRecord.Steps.Add(new TestingStep()
            {
                Title = "变压器放电"
            });
            TestingRecord.Steps.Add(new TestingStep()
            {
                Title = "变压器放电"
            });
            TestingRecord.Steps.Add(new TestingStep()
            {
                Title = "变压器放电"
            });
            TestingRecord.Steps.Add(new TestingStep()
            {
                Title = "变压器放电"
            });
            TestingRecord.Steps.Add(new TestingStep()
            {
                Title = "变压器放电"
            });
            TestingRecord.Steps.Add(new TestingStep()
            {
                Title = "变压器放电"
            });
            TestingRecord.Steps.Add(new TestingStep()
            {
                Title = "变压器放电"
            });
            TestingRecord.Steps.Add(new TestingStep()
            {
                Title = "变压器放电"
            });
            TestingRecord.Steps.Add(new TestingStep()
            {
                Title = "变压器放电"
            });
            TestingRecord.Steps.Add(new TestingStep()
            {
                Title = "变压器放电"
            });
            TestingRecord.Steps.Add(new TestingStep()
            {
                Title = "变压器放电"
            });
            TestingRecord.Steps.Add(new TestingStep()
            {
                Title = "变压器放电"
            });
            TestingRecord.Steps.Add(new TestingStep()
            {
                Title = "变压器放电"
            });
            TestingRecord.Steps.Add(new TestingStep()
            {
                Title = "变压器放电"
            });
            TestingRecord.Steps.Add(new TestingStep()
            {
                Title = "变压器放电"
            });
            TestingRecord.Steps.Add(new TestingStep()
            {
                Title = "变压器放电"
            });

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
