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
    /// Логика взаимодействия для RegistrationForm.xaml
    /// </summary>
    public partial class RegistrationForm : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly CustomerLogic logic;
        public int Id { set { id = value; } }
        private int? id;
        private void WindowRegistration_Loaded(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var view = logic.Read(new CustomerBindingModel { Id = id })?[0];
                    if (view != null)
                    {
                        name.Text = view.CustomerName;
                        secondName.Text = view.CustomerSurname;
                        tel.Text = view.Telephone;
                        password.Password = view.Password;
                        pathronic.Text = view.Patronymic;
                        email.Text = view.Email;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        public RegistrationForm(CustomerLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
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
            try
            {
                logic.CreateOrUpdate(new CustomerBindingModel
                {
                    Id = id,
                    CustomerName = name.Text,
                    CustomerSurname = secondName.Text,
                    Patronymic = pathronic.Text,
                    Telephone = tel.Text,
                    Email = email.Text,
                    Password = password.Password,
                    UserType = "Заказчик"
                });
                MessageBox.Show("Вы зарегитрировались как заказчик!", "Сообщение");
                var view = logic.Read(new CustomerBindingModel
                {
                    Telephone = tel.Text,
                    Password = password.Password
                });
                if (view != null && view.Count > 0)
                {
                    //DialogResult = true;
                    var window = Container.Resolve<MainWindowCustomer>();
                    window.Id = view[0].Id;
                    Close();
                    window.ShowDialog();
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
