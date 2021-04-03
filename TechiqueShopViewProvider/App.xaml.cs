using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TechiqueShopBusinessLogic.Interfaces;
using TechiqueShopDatabaseImplement.Implements;
using TechniqueShopBusinessLogic.BusinessLogics;
using Unity;
using Unity.Lifetime;

namespace TechiqueShopViewProvider
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<IProviderStorage, ProviderStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ProviderLogic>(new HierarchicalLifetimeManager());
            var mainWindow = currentContainer.Resolve<AuthorizationForm>();
            Application.Current.MainWindow = mainWindow;
            Application.Current.MainWindow.Show();
        }
    }
}
