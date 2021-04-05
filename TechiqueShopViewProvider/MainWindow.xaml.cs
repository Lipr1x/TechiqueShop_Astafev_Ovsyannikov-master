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
        public new IUnityContainer Container { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void exit_Button(object sender, RoutedEventArgs e)
        {
            AuthorizationForm form = Container.Resolve<AuthorizationForm>();
            form.Show();
            this.Close();
        }
    }
}
