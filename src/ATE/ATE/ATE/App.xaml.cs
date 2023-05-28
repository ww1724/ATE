using ATE.Common.Mvvm;
using ATE.Modules.ModuleName;
using ATE.Services;
using ATE.Services.Interfaces;
using ATE.Share;
using ATE.Stores;
using ATE.ViewModels;
using ATE.ViewModels.Admin;
using ATE.Views;
using ATE.Views.Admin;
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
            // Stores
            containerRegistry.RegisterSingleton<IViewModel, GlobalStore>();
            containerRegistry.RegisterSingleton<IViewModel, TestingStore>();

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

            // regions
            ContainerLocator.Container.Resolve<RegionManager>().RegisterViewWithRegion(Constants.MainRegion, Constants.TestBoard);
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ModuleNameModule>();
        }
    }
}
