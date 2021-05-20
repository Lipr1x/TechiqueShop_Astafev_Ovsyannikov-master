using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TechiqueShopBusinessLogic.BindingModels;
using TechiqueShopBusinessLogic.BusinessLogics;
using Unity;

namespace TechiqueShopViewProvider
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public int Id { set { id = value; } }
        private int? id;
        private ProviderLogic logic;
        public MainWindow(ProviderLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }
        private void MainWindowCustomer_Loaded(object sender, RoutedEventArgs e)
        {
            var client = logic.Read(new ProviderBindingModel { Id = id })?[0];
            labelCustomer.Content = "Клиент: " + client.ProviderName + " " + client.ProviderSurname;
        }
        private void Component_Button(object sender, RoutedEventArgs e)
        {
            ComponentForm form = Container.Resolve<ComponentForm>();
            form.Id = (int)id;
            form.ShowDialog();
        }
        private void Assembly_Button(object sender, RoutedEventArgs e)
        {
            AssemblyForm form = Container.Resolve<AssemblyForm>();
            form.Id = (int)id;
            form.ShowDialog();
        }

        private void Delivery_Button(object sender, RoutedEventArgs e)
        {
            DeliveryForm form = Container.Resolve<DeliveryForm>();
            form.Id = (int)id;
            form.ShowDialog();
        }

        private void GetList_Button(object sender, RoutedEventArgs e)
        {
            GetListForm form = Container.Resolve<GetListForm>();
            form.ShowDialog();
        }

        private void GetReport_Button(object sender, RoutedEventArgs e)
        {
            GetReportForm form = Container.Resolve<GetReportForm>();
            form.Show();
        }
    }
}
