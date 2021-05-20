using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
using TechiqueShopBusinessLogic.ViewModels;
using Unity;

namespace TechiqueShopViewProvider
{
    /// <summary>
    /// Логика взаимодействия для CreateDeliveryForm.xaml
    /// </summary>
    public partial class CreateDeliveryForm : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        public int ProviderId { set { providerId = value; } }

        private readonly DeliveryLogic logic;

        private int? id;

        private int? providerId;

        private Dictionary<int, (string, int)> deliveryComponents;

        public CreateDeliveryForm(DeliveryLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void CreateDeliveryForm_Loaded(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    DeliveryViewModel view = logic.Read(new DeliveryBindingModel { Id = id.Value })?[0];
                    if (view != null)
                    {
                        TextBoxIssueDate.Text = view.Date.ToString();
                        deliveryComponents = view.DeliveryComponents;
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
                deliveryComponents = new Dictionary<int, (string, int)>();
            }
        }

        private void LoadData()
        {
            try
            {
                if (deliveryComponents != null)
                {
                    dataGrid.Columns.Clear();
                    var list = new List<DataGridDeliveryItemViewModel>();
                    foreach (var dc in deliveryComponents)
                    {
                        list.Add(new DataGridDeliveryItemViewModel()
                        {
                            Id = dc.Key,
                            DeliveryName = dc.Value.Item1,
                            Count = dc.Value.Item2
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
            TextBoxIssueDate.Text = (DateTime.Now).ToString();
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<SelectComponentForm>();
            window.ProviderId = (int)providerId;
            if (window.ShowDialog().Value)
            {
                if (deliveryComponents.ContainsKey(window.Id))
                {
                    deliveryComponents[window.Id] = (window.OrderName, window.Count);
                }
                else
                {
                    deliveryComponents.Add(window.Id, (window.OrderName, window.Count));
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedCells.Count != 0)
            {
                var window = Container.Resolve<SelectComponentForm>();
                window.Id = ((DataGridDeliveryItemViewModel)dataGrid.SelectedCells[0].Item).Id;
                window.Count = ((DataGridDeliveryItemViewModel)dataGrid.SelectedCells[0].Item).Count;
                window.ProviderId = (int)providerId;
                if (window.ShowDialog().Value)
                {
                    deliveryComponents[window.Id] = (window.OrderName, window.Count);
                    LoadData();
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
                        deliveryComponents.Remove(((DataGridDeliveryItemViewModel)dataGrid.SelectedCells[0].Item).Id);
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
            if (deliveryComponents == null || deliveryComponents.Count == 0)
            {
                MessageBox.Show("Заполните косметику", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                logic.CreateOrUpdate(new DeliveryBindingModel
                {
                    Id = id,
                    Date = DateTime.Now,
                    DeliveryName = nameDelivery.Text,
                    DeliveryComponents = deliveryComponents,
                    ProviderId = providerId
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
