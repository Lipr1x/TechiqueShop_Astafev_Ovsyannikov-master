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
        public new IUnityContainer Container { get; set; }
        //public int Id { set { id = value; } }
        //private readonly ProviderStorage logicP;
        //private readonly CustomerStorage logicC;
        //private int? id;

        //public RegistrationForm(ProviderStorage logicP, CustomerStorage logicC)
        //{
        //    InitializeComponent();
        //    this.logicP = logicP;
        //    this.logicC = logicC;
        //}
        //private void Accept_Click(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrEmpty(name.Text))
        //    {
        //        MessageBox.Show("Заполните имя!");
        //        return;
        //    }
        //    if (string.IsNullOrEmpty(secondName.Text))
        //    {
        //        MessageBox.Show("Заполните фамилию!");
        //        return;
        //    }
        //    if (string.IsNullOrEmpty(pathronic.Text))
        //    {
        //        MessageBox.Show("Заполните отчество!");
        //        return;
        //    }
        //    if (string.IsNullOrEmpty(tel.Text))
        //    {
        //        MessageBox.Show("Заполните телефон!");
        //        return;
        //    }
        //    if (string.IsNullOrEmpty(email.Text))
        //    {
        //        MessageBox.Show("Заполните E-mail!");
        //        return;
        //    }
        //    if (string.IsNullOrEmpty(password.Password))
        //    {
        //        MessageBox.Show("Заполните пароль!");
        //        return;
        //    }
        //    if (userType.SelectedIndex == -1)
        //    {
        //        MessageBox.Show("Выберите роль!");
        //        return;
        //    }
        //    try
        //    {
        //        if (userType.Text.Equals("Поставщик"))
        //        {
        //            logicP.Insert(new ProviderBindingModel
        //            {
        //                Id = id,
        //                ProviderName = name.Text,
        //                ProviderSurname = secondName.Text,
        //                Patronymic = pathronic.Text,
        //                Telephone = tel.Text,
        //                Email = email.Text,
        //                Password = password.Password,
        //                UserType = userType.Text
        //            });
        //            MessageBox.Show("Вы зарегитрировались как поставщик!");
        //        }
        //        else
        //        {
        //            logicC.Insert(new CustomerBindingModel
        //            {
        //                Id = id,
        //                CustomerName = name.Text,
        //                CustomerSurname = secondName.Text,
        //                Patronymic = pathronic.Text,
        //                Telephone = tel.Text,
        //                Email = email.Text,
        //                Password = password.Password,
        //                UserType = userType.Text
        //            });
        //            MessageBox.Show("Вы зарегитрировались как заказчик!");
        //        }
        //        this.DialogResult = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Произошла ошибка!");
        //    }
        }
    }

