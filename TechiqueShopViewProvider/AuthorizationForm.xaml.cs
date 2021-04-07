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
using TechiqueShopDatabaseImplement;
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
        private readonly TechiqueShopDatabase db = new TechiqueShopDatabase();
        public AuthorizationForm(ProviderLogic logicP, CustomerLogic logicC)
        {
            InitializeComponent();
            this.logicP = logicP;
            this.logicC = logicC;
        }

        //Вывод пользователей в консоль
        private void OutputUsers()
        {
            var usersP = db.Providers.ToList();
            Console.WriteLine("Поставщики:");
            foreach (var user in usersP)
            {
                Console.WriteLine($"{user.Id}|{user.ProviderName}|{user.Patronymic}|" +
                    $"{user.ProviderSurname}|{user.Telephone}|{user.Email}|{user.Password}");
            }

            var usersC = db.Customers.ToList();
            Console.WriteLine("Заказчики:");
            foreach (var user in usersC)
            {
                Console.WriteLine($"{user.Id}|{user.CustomerName}|{user.Patronymic}|" +
                    $"{user.CustomerSurname}|{user.Telephone}|{user.Email}|{user.Password}");
            }
        }

        // Обработка кнопки перехода на форму регистрации
        private void Regis_Click(object sender, RoutedEventArgs e)
        {
            OutputUsers();

            var form = Container.Resolve<RegistrationForm>();
            Close();
            form.ShowDialog();
        }

        // Обработка кнопки входа
        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(login.Text))
            {
                MessageBox.Show("Введите логин!", "Ошибка");
                return;
            }
            if (string.IsNullOrEmpty(password.Password))
            {
                MessageBox.Show("Введите пароль!", "Ошибка");
                return;
            }
            if (userType.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите роль!", "Ошибка");
                return;
            }
            try
            {
                if(userType.Text == "Поставщик") { 
                    logicP.Login(new ProviderBindingModel
                    {
                        Telephone = login.Text,
                        Password = password.Password,
                    });
                    var form = Container.Resolve<MainWindow>();
                    Close();
                    form.ShowDialog();
                }
                else if(userType.Text == "Заказчик") { 
                    logicC.Login(new CustomerBindingModel
                    {
                        Telephone = login.Text,
                        Password = password.Password,
                    });
                    var form = Container.Resolve<MainWindowCustomer>();
                    Close();
                    form.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }

        }
    }
}
