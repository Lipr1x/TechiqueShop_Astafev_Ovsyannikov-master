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

namespace TechiqueShopViewProvider
{
    /// <summary>
    /// Логика взаимодействия для CreateDeliveryForm.xaml
    /// </summary>
    public partial class CreateDeliveryForm : Window
    {
        private readonly DeliveryLogic delivery_logic;
        private readonly ComponentLogic component_logic;
        public int Id { set { id = value; } }
        private int? id;
        private Dictionary<int, string> deliveryComponents;
        public CreateDeliveryForm(DeliveryLogic _delivery_logic, ComponentLogic _component_logic)
        {
            InitializeComponent();
            this.delivery_logic = _delivery_logic;
            this.component_logic = _component_logic;
            this.Loaded += CreateDeliveryForm_Loaded;
        }
        private void CreateDeliveryForm_Loaded(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    DeliveryViewModel view = delivery_logic.Read(new DeliveryBindingModel
                    {
                        Id = id.Value
                    })?[0];
                    deliveryComponents = view.DeliveryComponents;
                    nameDelivery.Text = view.DeliveryName.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
                }
            }
            else
            {
                deliveryComponents = new Dictionary<int, string>();
            }
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                if (deliveryComponents != null)
                {
                    SelectedComponents.Items.Clear();
                    //dataGridView.Columns[0].Visible = false;
                    foreach (var comp in deliveryComponents)
                    {
                        SelectedComponents.Items.Add(comp.Value);
                    }
                }
                CanSelectedComponents.Items.Clear();
                foreach (var m in component_logic.Read(null))
                {
                    if (deliveryComponents.Values.Where(rec => rec == m.ComponentName).ToList().Count == 0)
                    {
                        CanSelectedComponents.Items.Add(m);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(nameDelivery.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
                return;
            }
            if (deliveryComponents == null || deliveryComponents.Count == 0)
            {
                MessageBox.Show("Заполните компоненты", "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
                return;
            }
            try
            {
                delivery_logic.CreateOrUpdate(new DeliveryBindingModel
                {
                    Id = id,
                    DeliveryName = nameDelivery.Text,
                    Price = Convert.ToInt32(priceDelivery.Text),
                    DeliveryComponents = deliveryComponents,
                    UserId = 1
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK,
               MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
            }
        }

        private void RefundButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedComponents.SelectedItems.Count == 1)
            {
                deliveryComponents.Remove(deliveryComponents.Where(rec => rec.Value == (string)SelectedComponents.SelectedItem).ToList()[0].Key);
                LoadData();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (CanSelectedComponents.SelectedItems.Count == 1)
            {
                deliveryComponents.Add((int)((ComponentViewModel)CanSelectedComponents.SelectedItem).Id
                    , ((ComponentViewModel)CanSelectedComponents.SelectedItem).ComponentName);
                LoadData();
            }
        }
    }
}
