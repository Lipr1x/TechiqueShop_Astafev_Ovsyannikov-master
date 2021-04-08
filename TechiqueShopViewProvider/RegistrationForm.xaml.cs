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
//using TechiqueShopDatabaseImplement.Implements;
using Unity;

namespace TechiqueShopViewProvider
{
    /// <summary>
    /// Логика взаимодействия для RegistrationForm.xaml
    /// </summary>
    public partial class RegistrationForm : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly ProviderLogic logicP;
        private readonly CustomerLogic logicC;
        public int Id { set => id = value; }
        private int? id;

        public RegistrationForm(ProviderLogic logicP, CustomerLogic logicC)
        {
            InitializeComponent();
            this.logicP = logicP;
            this.logicC = logicC;
        }
        private void Accept_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(name.Text))
            {
                MessageBox.Show("Заполните имя!", "Ошибка");
                return;
            }
            if (string.IsNullOrEmpty(secondName.Text))
            {
                MessageBox.Show("Заполните фамилию!", "Ошибка");
                return;
            }
            if (string.IsNullOrEmpty(pathronic.Text))
            {
                MessageBox.Show("Заполните отчество!", "Ошибка");
                return;
            }
            if (string.IsNullOrEmpty(tel.Text))
            {
                MessageBox.Show("Заполните телефон!", "Ошибка");
                return;
            }
            if (string.IsNullOrEmpty(email.Text))
            {
                MessageBox.Show("Заполните E-mail!", "Ошибка");
                return;
            }
            if (string.IsNullOrEmpty(password.Password))
            {
                MessageBox.Show("Заполните пароль!", "Ошибка");
                return;
            }
            if (userType.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите роль!", "Ошибка");
                return;
            }
            try
            {
                if (userType.Text.Equals("Поставщик"))
                {
                    logicP.CreateOrUpdate(new ProviderBindingModel
                    {
                        Id = id,
                        ProviderName = name.Text,
                        ProviderSurname = secondName.Text,
                        Patronymic = pathronic.Text,
                        Telephone = tel.Text,
                        Email = email.Text,
                        Password = password.Password,
                        UserType = userType.Text
                    });
                    MessageBox.Show("Вы зарегитрировались как поставщик!", "Сообщение");
                    var form = Container.Resolve<MainWindow>();
                    Close();
                    form.ShowDialog();
                }
                else
                {
                    logicC.CreateOrUpdate(new CustomerBindingModel
                    {
                        Id = id,
                        CustomerName = name.Text,
                        CustomerSurname = secondName.Text,
                        Patronymic = pathronic.Text,
                        Telephone = tel.Text,
                        Email = email.Text,
                        Password = password.Password,
                        UserType = userType.Text
                    });
                    MessageBox.Show("Вы зарегитрировались как заказчик!", "Сообщение");
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
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            var form = Container.Resolve<AuthorizationForm>();
            Close();
            form.ShowDialog();
        }
    }
}
