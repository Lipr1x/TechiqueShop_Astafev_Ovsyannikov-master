using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using TechiqueShopBusinessLogic.BindingModels;
using TechiqueShopBusinessLogic.BusinessLogics;
using TechiqueShopBusinessLogic.ViewModels;
using Unity;

namespace TechiqueShopViewCustomer
{
    /// <summary>
    /// Логика взаимодействия для CreateSupplyForm.xaml
    /// </summary>
    public partial class CreateSupplyForm : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public int Id { set { id = value; } }
        private int? id;
        public int CustomerId { set { customerId = value; } }
        private int? customerId;

        private readonly SupplyLogic logicS;
        private readonly OrderLogic logicO;

        private Dictionary<int, (string, int)> supplyOrders;

        public CreateSupplyForm(SupplyLogic logicS, OrderLogic logicO)
        {
            InitializeComponent();
            this.logicS = logicS;
            this.logicO = logicO;
        }

        private void CreateSupplyForm_Loaded(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    SupplyViewModel view = logicS.Read(new SupplyBindingModel { Id = id.Value })?[0];
                    if (view != null)
                    {
                        dateSupply.Text = view.Date.ToString();
                        supplyOrders = view.SupplyOrders;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                supplyOrders = new Dictionary<int, (string, int)>();
            }
        }

        private void LoadData()
        {
            try
            {
                if (supplyOrders != null)
                {
                    dataGrid.Columns.Clear();
                    var list = new List<DataGridSupplyItemViewModel>();
                    foreach (var rc in supplyOrders)
                    {
                        list.Add(new DataGridSupplyItemViewModel()
                        {
                            Id = rc.Key,
                            OrderName = rc.Value.Item1,
                            Price = logicO.Read(new OrderBindingModel { Id = rc.Key })?[0].Price,
                            Count = rc.Value.Item2
                        });
                    }
                    dataGrid.ItemsSource = list;
                    dataGrid.Columns[0].Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            dateSupply.Text = (DateTime.Now).ToString();
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<SelectOrderForm>();
            window.CustomerId = (int)customerId;
            if (window.ShowDialog().Value)
            {
                if (supplyOrders.ContainsKey(window.Id))
                {
                    supplyOrders[window.Id] = (window.OrderName, window.Count);
                }
                else
                {
                    supplyOrders.Add(window.Id, (window.OrderName, window.Count));
                }
                LoadData();
                CalcTotalCost();
            }
        }

        private void CalcTotalCost()
        {
            try
            {
                int totalCost = 0;
                foreach (var so in supplyOrders)
                {
                    totalCost += so.Value.Item2 * (int)logicO.Read(new OrderBindingModel { Id = so.Key })?[0].Price;
                }
                totalCostSupply.Text = totalCost.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonUpd_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedCells.Count != 0)
            {
                var window = Container.Resolve<SelectOrderForm>();
                window.Id = ((DataGridSupplyItemViewModel)dataGrid.SelectedCells[0].Item).Id;
                window.Count = ((DataGridSupplyItemViewModel)dataGrid.SelectedCells[0].Item).Count;
                window.CustomerId = (int)customerId;

                if (window.ShowDialog().Value)
                {
                    supplyOrders[window.Id] = (window.OrderName, window.Count);
                    LoadData();
                    CalcTotalCost();
                }
            }
        }

        private void buttonDel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedCells.Count != 0)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        supplyOrders.Remove(((DataGridSupplyItemViewModel)dataGrid.SelectedCells[0].Item).Id);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRef_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (supplyOrders == null || supplyOrders.Count == 0)
            {
                MessageBox.Show("Заполните косметику", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                logicS.CreateOrUpdate(new SupplyBindingModel
                {
                    Id = id,
                    Date = DateTime.Now,
                    SupplyName = nameSupply.Text,
                    TotalCost = Convert.ToDecimal(totalCostSupply.Text),
                    SupplyOrders = supplyOrders,
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

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
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
