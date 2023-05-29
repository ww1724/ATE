using ATE.Common.Mvvm;
using ATE.Service;
using ATE.Service.Interface;
using ATE.Services.Entities;
using ATE.Share;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using Zoranof.Workflow;
using Zoranof.Workflow.Common;
using Zoranof.WorkFlow;

namespace ATE.ViewModels.Admin
{
    public class TestintProjectEditorViewModel : BindableBase, IViewModel
    {
        private IDbService DbService { get; set; }
        private IDialogService DialogService { get; set; }

        private ObservableCollection<NodeItemMeta> nodeItemMetas;
        private ObservableCollection<TestingProjectV> projects;
        private int editingProjectID;
        private TestingProjectV editingProjectV;

        public ObservableCollection<NodeItemMeta> NodeItemMetas { get => nodeItemMetas; set { SetProperty(ref nodeItemMetas, value); } }

        public int EditingProjectID { get => editingProjectID; set { SetProperty(ref editingProjectID, value); } }

        public TestingProjectV EditingProjectV { get => editingProjectV; set { SetProperty(ref editingProjectV, value); } }

        public ObservableCollection<TestingProjectV> Projects { get => projects; set { SetProperty(ref projects, value); } }

        public TestintProjectEditorViewModel(IDbService dbService, IDialogService dialogService)
        {
            // ctor init
            DialogService = dialogService;

            #region QueryProjects
            DbService = dbService;
            EditingProjectID = 0;

            var projects = DbService.Query<TestingProjectEntity>();

            #endregion

            #region LoadAssemblies
            var t = new ObservableCollection<NodeItemMeta>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var nodeTypes = assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(WorkflowNode))).ToList();
                foreach (var x in nodeTypes)
                {
                    var attr = x.GetCustomAttribute<NodeAttribute>();
                    if (attr == null || attr.Group == "Template") continue;
                    var itemMeta = new NodeItemMeta();
                    itemMeta.Type = x;
                    itemMeta.Guid = x.GUID.ToString();
                    itemMeta.Title = attr != null ? attr.Title : x.Name;
                    itemMeta.Group = attr != null ? attr.Group : "Base";
                    itemMeta.Attrs = x.GetCustomAttributes(true).ToDictionary(x => x.GetType().Name, x => x);
                    t.Add(itemMeta);
                }
            }
            NodeItemMetas = new ObservableCollection<NodeItemMeta>(t);
            #endregion

            #region Actions & Commands
            NewTestingProjectDialogCommand = new DelegateCommand(NewTestingProjectCommandDialogAction);
            NewTestingProjectCommand = new DelegateCommand<string>(NewTestingProjectCommandAction);
            #endregion

        }

        public DelegateCommand NewTestingProjectDialogCommand { get;set; }

        public DelegateCommand<string> NewTestingProjectCommand { get; set; }

        public void NewTestingProjectCommandDialogAction()
        {
            DialogService.ShowDialog(Constants.NewProjectDialog, new DialogParameters(), (IDialogResult dr) =>
            {
                if (dr.Result == ButtonResult.Yes)
                {
                    string title = dr.Parameters.GetValue<string>("Name");
                    DbService.Insert<TestingProjectEntity>(new TestingProjectEntity
                    {
                        Title = title,
                        ProjectId = Guid.NewGuid().ToString(),
                        ClassType = ""
                    });
                }

            });
        }

        public void NewTestingProjectCommandAction(string name)
        {

        }
    }
}
