using TechiqueShopBusinessLogic.BindingModels;
using TechiqueShopBusinessLogic.BusinessLogics;
using TechiqueShopDatabaseImplement.Models;
using System.Windows;
using System.Windows.Controls;
using Unity;
using System;
using System.ComponentModel;
using System.Reflection;

namespace TechiqueShopViewCustomer
{
    /// <summary>
    /// Логика взаимодействия для OrderForm.xaml
    /// </summary>
    public partial class OrderForm : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public int Id { set { id = value; } }
        private int? id;
        private readonly OrderLogic logic;
        public OrderForm(OrderLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }
        private void OrderForm_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                var list = logic.Read(new OrderBindingModel { CustomerId = id });
                if (list != null)
                {
                    orderDataGrid.ItemsSource = list;
                    orderDataGrid.Columns[0].Visibility = Visibility.Hidden;
                    orderDataGrid.Columns[1].Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<CreateOrderForm>();
            window.CustomerId = (int)id;
            if (window.ShowDialog().Value)
            {
                LoadData();
            }
        }
        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (orderDataGrid.SelectedItems.Count == 1)
            {
                var window = Container.Resolve<CreateOrderForm>();
                window.Id = ((OrderViewModel)orderDataGrid.SelectedItem).Id;
                window.CustomerId = (int)id;
                if (window.ShowDialog().Value)
                {
                    LoadData();
                }
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (orderDataGrid.SelectedItems.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        int id = Convert.ToInt32(((ProviderViewModel)orderDataGrid.SelectedItem).Id);
                        logic.Delete(new OrderBindingModel { Id = id });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                       MessageBoxImage.Error);
                    }
                    LoadData();
                }
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        /// <summary>
        /// Данные для привязки DisplayName к названиям столбцов
        /// </summary>
        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string displayName = GetPropertyDisplayName(e.PropertyDescriptor);
            if (!string.IsNullOrEmpty(displayName))
            {
                e.Column.Header = displayName;
            }
        }

        /// <summary>
        /// метод привязки DisplayName к названию столбца
        /// </summary>
        public static string GetPropertyDisplayName(object descriptor)
        {
            PropertyDescriptor pd = descriptor as PropertyDescriptor;
            if (pd != null)
            {
                // Check for DisplayName attribute and set the column header accordingly
                DisplayNameAttribute displayName = pd.Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute;
                if (displayName != null && displayName != DisplayNameAttribute.Default)
                {
                    return displayName.DisplayName;
                }
            }
            else
            {
                PropertyInfo pi = descriptor as PropertyInfo;
                if (pi != null)
                {
                    // Check for DisplayName attribute and set the column header accordingly
                    Object[] attributes = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                    for (int i = 0; i < attributes.Length; ++i)
                    {
                        DisplayNameAttribute displayName = attributes[i] as DisplayNameAttribute;
                        if (displayName != null && displayName != DisplayNameAttribute.Default)
                        {
                            return displayName.DisplayName;
                        }
                    }
                }
            }
            return null;
        }
    }
}
