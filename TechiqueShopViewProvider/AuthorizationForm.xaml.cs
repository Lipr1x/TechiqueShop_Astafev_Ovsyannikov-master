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

namespace TechiqueShopViewProvider
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationForm.xaml
    /// </summary>
    public partial class AuthorizationForm : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        public AuthorizationForm()
        {
            InitializeComponent();
        }

        // Обработка кнопки перехода на форму регистрации
        private void regis_Click(object sender, RoutedEventArgs e)
        {
            RegistrationForm form = Container.Resolve<RegistrationForm>();

            form.Show();
        }

        // Обработка кнопки входа
        private void enter_Click(object sender, RoutedEventArgs e)
        {
            if (login.Text.Length > 8) // проверяем введён ли логин     
            {
                if (password.Password.Length > 8) // проверяем введён ли пароль         
                {             // ищем в базе данных пользователя с такими данными         
                    ///DataTable dt_user = mainWindow.Select("SELECT * FROM [dbo].[users] WHERE [login] = '" + textBox_login.Text + "' AND [password] = '" + password.Password + "'");
                    if (true)//dt_user.Rows.Count > 0) // если такая запись существует       
                    {
                        MessageBox.Show("Пользователь авторизовался"); // говорим, что авторизовался  
                        MainWindow form = Container.Resolve<MainWindow>();
                        form.Show();
                    }
                    else MessageBox.Show("Пользователя не найден"); // выводим ошибку  
                }
                else MessageBox.Show("Введите пароль"); // выводим ошибку    
            }
            else MessageBox.Show("Введите логин"); // выводим ошибку 

        }
    }
}
