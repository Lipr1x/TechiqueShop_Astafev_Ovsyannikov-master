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
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Component_Button(object sender, RoutedEventArgs e)
        {
            ComponentForm form = Container.Resolve<ComponentForm>();
            form.ShowDialog();
        }
        private void Assembly_Button(object sender, RoutedEventArgs e)
        {
            AssemblyForm form = Container.Resolve<AssemblyForm>();
            form.Show();
        }

        private void Delivery_Button(object sender, RoutedEventArgs e)
        {
            DeliveryForm form = Container.Resolve<DeliveryForm>();
            form.Show();
        }

        private void GetList_Button(object sender, RoutedEventArgs e)
        {
            GetListForm form = Container.Resolve<GetListForm>();
            form.Show();
        }

        private void GetReport_Button(object sender, RoutedEventArgs e)
        {
            GetReportForm form = Container.Resolve<GetReportForm>();
            form.Show();
        }
        private void Exit_Button(object sender, RoutedEventArgs e)
        {
            AuthorizationForm form = Container.Resolve<AuthorizationForm>();
            form.Show();
            this.Close();
        }
    }
}
