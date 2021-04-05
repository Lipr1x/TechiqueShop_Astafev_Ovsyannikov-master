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
using Unity;

namespace TechiqueShopViewCustomer
{
    /// <summary>
    /// Логика взаимодействия для MainWindowCustomer.xaml
    /// </summary>
    public partial class MainWindowCustomer : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        public MainWindowCustomer()
        {
            InitializeComponent();
        }
        //Заказы
        private void order_Button(object sender, RoutedEventArgs e)
        {
            OrderForm form = Container.Resolve<OrderForm>();
            form.Show();
        }
        //Поставки
        private void supply_Button(object sender, RoutedEventArgs e)
        {
            SupplyForm form = Container.Resolve<SupplyForm>();
            form.Show();
        }
        //Техника
        private void technique_Button(object sender, RoutedEventArgs e)
        {
            TechniqueForm form = Container.Resolve<TechniqueForm>();
            form.Show();
        }
        //Получение списка
        private void getList_Button(object sender, RoutedEventArgs e)
        {
            GetListForm form = Container.Resolve<GetListForm>();
            form.Show();
        }
        //Отчет
        private void getReport_Button(object sender, RoutedEventArgs e)
        {
            GetReportForm form = Container.Resolve<GetReportForm>();
            form.Show();
        }
        //Выход на авторизацию
        private void exit_Button(object sender, RoutedEventArgs e)
        {
            //AuthorizationForm form = Container.Resolve<AuthorizationForm>();
            //form.Show();
            this.Close();
        }
    }
}
