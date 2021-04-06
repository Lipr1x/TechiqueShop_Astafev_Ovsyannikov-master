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
using TechiqueShopViewCustomer;
using Unity;

namespace TechiqueShopViewProvider
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationForm.xaml
    /// </summary>
    public partial class AuthorizationForm : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly ProviderLogic logicP;
        private readonly CustomerLogic logicC;
        public AuthorizationForm(ProviderLogic logicP, CustomerLogic logicC)
        {
            InitializeComponent();
            this.logicP = logicP;
            this.logicC = logicC;
        }

        // Обработка кнопки перехода на форму регистрации
        private void regis_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<RegistrationForm>();
            Close();
            form.ShowDialog();
        }
        private void cust_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<MainWindowCustomer>();
            Close();
            form.ShowDialog();
        }

        // Обработка кнопки входа
        private void enter_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(login.Text))
            {
                MessageBox.Show("Введите логин", "Ошибка");
                return;
            }
            if (string.IsNullOrEmpty(password.Password))
            {
                MessageBox.Show("Введите пароль", "Ошибка");
                return;
            }
            try
            {
                logicP.Login(new ProviderBindingModel
                {
                    Telephone = login.Text,
                    Password = password.Password,
                });
                var form = Container.Resolve<MainWindow>();
                Close();
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }

        }
    }
}
