using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TechiqueShopBusinessLogic.BindingModels;
using TechiqueShopBusinessLogic.BusinessLogics;
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
        private readonly ProviderLogic logic;
        public int Id { set => id = value; }
        private int? id;

        public RegistrationForm(ProviderLogic logic)
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

                logic.CreateOrUpdate(new ProviderBindingModel
                {
                    Id = id,
                    ProviderName = name.Text,
                    ProviderSurname = secondName.Text,
                    Patronymic = pathronic.Text,
                    Telephone = tel.Text,
                    Email = email.Text,
                    Password = password.Password,
                    UserType = "Производитель"
                });
                MessageBox.Show("Вы зарегитрировались, как производитель!", "Сообщение");
                var view = logic.Read(new ProviderBindingModel
                {
                    Telephone = tel.Text,
                    Password = password.Password
                });
                if (view != null && view.Count > 0)
                {
                    //DialogResult = true;
                    var window = Container.Resolve<MainWindow>();
                    window.Id = (int)view[0].Id;
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
