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
using System.Windows.Shapes;
using TechiqueShopBusinessLogic.BindingModels;
using TechiqueShopBusinessLogic.BusinessLogics;
using Unity;

namespace TechiqueShopViewCustomer
{
    /// <summary>
    /// Логика взаимодействия для MainWindowCustomer.xaml
    /// </summary>
    public partial class MainWindowCustomer : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public int Id { set { id = value; } }
        private int? id;
        private CustomerLogic logic;
        public MainWindowCustomer(CustomerLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }
        private void MainWindowCustomer_Loaded(object sender, RoutedEventArgs e)
        {
            var client = logic.Read(new CustomerBindingModel { Id = id })?[0];
            labelCustomer.Content = "Клиент: " + client.CustomerName + " " + client.CustomerSurname;
        }
        //Заказы
        private void Order_Button(object sender, RoutedEventArgs e)
        {
            OrderForm form = Container.Resolve<OrderForm>();
            form.Id = (int)id;
            form.ShowDialog();
        }
        //Поставки
        private void Supply_Button(object sender, RoutedEventArgs e)
        {
            SupplyForm form = Container.Resolve<SupplyForm>();
            form.Id = (int)id;
            form.ShowDialog();
        }
        //Техника
        private void Technique_Button(object sender, RoutedEventArgs e)
        {
            TechniqueForm form = Container.Resolve<TechniqueForm>();
            form.Id = (int)id;
            form.ShowDialog();
        }
        //Получение списка
        private void GetList_Button(object sender, RoutedEventArgs e)
        {
            GetListForm form = Container.Resolve<GetListForm>();
            form.ShowDialog();
        }
        //Отчет
        private void GetReport_Button(object sender, RoutedEventArgs e)
        {
            GetReportForm form = Container.Resolve<GetReportForm>();
            form.ShowDialog();
        }
    }
}
