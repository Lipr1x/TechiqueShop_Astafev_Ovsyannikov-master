using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
using TechiqueShopDatabaseImplement;
using TechiqueShopDatabaseImplement.Models;
using Unity;

namespace TechiqueShopViewCustomer
{
    /// <summary>
    /// Логика взаимодействия для CreateOrderForm.xaml
    /// </summary>
    public partial class CreateOrderForm : Window
    {
        TechiqueShopDatabase db;
        public CreateOrderForm()
        {
            InitializeComponent();
            db = new TechiqueShopDatabase();
        }

        private void Save_button(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(name.Text))
                {
                    MessageBox.Show("Заполните поле Название", "Ошибка");
                    return;
                }
                Order order = new Order
                {
                    OrderName = name.Text,
                    Price = Convert.ToInt32(price.Text)
                };

                // Добавить в DbSet
                db.Orders.Add(order);

                // Сохранить изменения в базе данных
                db.SaveChanges();
            }
            catch
            {
                MessageBox.Show("Произошла ошибка", "Ошибка");
            }
        }
        private void Close_button(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
