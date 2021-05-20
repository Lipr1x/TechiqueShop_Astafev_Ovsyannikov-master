using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TechiqueShopBusinessLogic.BusinessLogics;
using TechiqueShopBusinessLogic.Interfaces;
using TechiqueShopDatabaseImplement.Implements;
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
            currentContainer.RegisterType<IComponentStorage, ComponentStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ComponentLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ICustomerStorage, CustomerStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<CustomerLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IAssemblyStorage, AssemblyStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<AssemblyLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IDeliveryStorage, DeliveryStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<DeliveryLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IGetTechniqueStorage, GetTechniqueStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<GetTechniqueLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IOrderStorage, OrderStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<OrderLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ISupplyStorage, SupplyStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<SupplyLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IProviderStorage, ProviderStorage>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ProviderLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<GetListLogic>(new HierarchicalLifetimeManager());
            var mainWindow = currentContainer.Resolve<AuthorizationForm>();
            Application.Current.MainWindow = mainWindow;
            Application.Current.MainWindow.Show();
        }
    }
}
