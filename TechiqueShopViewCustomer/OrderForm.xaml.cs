using Microsoft.EntityFrameworkCore;
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
using TechiqueShopDatabaseImplement;
using TechiqueShopDatabaseImplement.Models;
using Unity;

namespace TechiqueShopViewCustomer
{
    /// <summary>
    /// Логика взаимодействия для OrderForm.xaml
    /// </summary>
    public partial class OrderForm : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly TechiqueShopDatabase db;
        public OrderForm()
        {
            InitializeComponent();

            db = new TechiqueShopDatabase();
            db.Orders.Load(); // загружаем данные
            ordersGrid.ItemsSource = db.Orders.Local.ToBindingList(); // устанавливаем привязку к кэшу

            this.Closing += OrderForm_Closing;
        }

        private void OrderForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            db.Dispose();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            db.SaveChanges();
        }
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            CreateOrderForm form = Container.Resolve<CreateOrderForm>();
            form.Show();
        }
        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            CreateOrderForm form = Container.Resolve<CreateOrderForm>(); ;
            form.Show();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ordersGrid.SelectedItems.Count > 0)
            {
                for (int i = 0; i < ordersGrid.SelectedItems.Count; i++)
                {
                    Order phone = ordersGrid.SelectedItems[i] as Order;
                    if (phone != null)
                    {
                        db.Orders.Remove(phone);
                    }
                }
            }
            db.SaveChanges();
        }
    }
}
