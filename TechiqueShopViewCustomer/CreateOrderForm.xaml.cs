using TechiqueShopBusinessLogic.BindingModels;
using TechiqueShopBusinessLogic.BusinessLogics;
using System.Windows;
using Unity;
using System;

namespace TechiqueShopViewCustomer
{
    /// <summary>
    /// Логика взаимодействия для CreateOrderForm.xaml
    /// </summary>
    public partial class CreateOrderForm : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly OrderLogic logic;
        public int Id { set => id = value; }
        private int? id;
        public int CustomerId { set { customerId = value; } }
        private int? customerId;
        public CreateOrderForm(OrderLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }
        private void CreateOrderForm_Loaded(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var view = logic.Read(new OrderBindingModel { Id = id })?[0];
                    if (view != null)
                    {
                        name.Text = view.OrderName;
                        price.Text = view.Price.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void Save_button(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(name.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(price.Text))
            {
                MessageBox.Show("Внесите стоимость", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                logic.CreateOrUpdate(new OrderBindingModel
                {
                    Id = id,
                    OrderName = name.Text,
                    Price = Convert.ToInt32(price.Text),
                    CustomerId = customerId
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Close_button(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
