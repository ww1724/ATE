using ATE.Common.Mvvm;
using ATE.Modules.ModuleName;
using ATE.Service;
using ATE.Service.Interface;
using ATE.Services;
using ATE.Services.Interfaces;
using ATE.Share;
using ATE.Stores;
using ATE.Test;
using ATE.ViewModels;
using ATE.ViewModels.Admin;
using ATE.ViewModels.Dialog;
using ATE.Views;
using ATE.Views.Admin;
using ATE.Views.Dialogs;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Windows;

namespace ATE
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public IServiceProvider ServiceProvider;

        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // services
            containerRegistry.RegisterSingleton<IMessageService, MessageService>();
            containerRegistry.RegisterSingleton<IDbService, DbService>();
            containerRegistry.RegisterSingleton<ILoggerService, LoggerService>();

            containerRegistry.RegisterSingleton<DeviceManager>();
            // Stores
            containerRegistry.RegisterSingleton<GlobalStore>();
            containerRegistry.RegisterSingleton<TestingStore>();
            // View Locator
            containerRegistry.RegisterSingleton<IViewModel, ShellViewModel>(Constants.Shell);
            containerRegistry.RegisterForNavigation<TestView, TestViewModel>(Constants.TestBoard);
            containerRegistry.RegisterForNavigation<EditView, EditViewModel>(Constants.EditBoard);
            containerRegistry.RegisterForNavigation<HistoryView, HistoryViewModel>(Constants.History);
            containerRegistry.RegisterForNavigation<AdminView, AdminViewModel>(Constants.Admin);
            containerRegistry.RegisterForNavigation<TestingProjectEditorView, TestintProjectEditorViewModel>(Constants.TestProject);
            containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>(Constants.Settings);
            containerRegistry.RegisterForNavigation<DeviceMangeView, DeviceManageViewModel>(Constants.DeviceManage);
            containerRegistry.RegisterForNavigation<DatabaseView, DatabaseViewModel>(Constants.Database);
            //dialogs
            containerRegistry.RegisterDialog<LoginView, LoginViewModel>(Constants.LoginDialog);
            containerRegistry.RegisterDialog<NewProjectDialog, NewProjectDialogViewModel>(Constants.NewProjectDialog);
            containerRegistry.RegisterDialog<ImportTestingProjectDialog, ImportTestingProjectDialogViewModel>(Constants.ImportTestingProjectDialog);
            containerRegistry.RegisterDialog<ScanTestingProjectDialog, ScanTestingProjectDialogViewModel>(Constants.ScanTestingProjectDialog);
            // regions
            ContainerLocator.Container.Resolve<RegionManager>().RegisterViewWithRegion(Constants.MainRegion, Constants.TestBoard);
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ModuleNameModule>();
        }
    }
}
