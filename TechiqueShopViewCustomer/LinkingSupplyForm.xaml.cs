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
using Unity;

namespace TechiqueShopViewCustomer
{
    /// <summary>
    /// Логика взаимодействия для LinkingSupplyForm.xaml
    /// </summary>
    public partial class LinkingSupplyForm : Window
    {
        private readonly SupplyLogic supply_logic;
        private readonly ComponentLogic component_logic;
        public int Id { set { id = value; } }
        private int? id;
        private Dictionary<int, string> supplyComponents;
        public LinkingSupplyForm(SupplyLogic _supply_logic, ComponentLogic _component_logic)
        {
            InitializeComponent();
            this.supply_logic = _supply_logic;
            this.component_logic = _component_logic;;
        }
        private void LinkingSupplyForm_Loaded(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    SupplyViewModel view = supply_logic.Read(new SupplyBindingModel
                    {
                        Id = id.Value
                    })?[0];
                    supplyComponents = view.SupplyComponents;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
                }
            }
            else
            {
                supplyComponents = new Dictionary<int, string>();
            }
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                if (supplyComponents != null)
                {
                    SelectedComponents.Items.Clear();
                    //dataGridView.Columns[0].Visible = false;
                    foreach (var comp in supplyComponents)
                    {
                        SelectedComponents.Items.Add(comp.Value);
                    }
                }
                CanSelectedComponents.Items.Clear();
                foreach (var m in component_logic.Read(null))
                {
                    if (supplyComponents.Values.Where(rec => rec == m.ComponentName).ToList().Count == 0)
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
            if (supplyComponents == null || supplyComponents.Count == 0)
            {
                MessageBox.Show("Заполните компоненты", "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
                return;
            }
            try
            {
                supply_logic.Linking(new SupplyBindingModel
                {
                    SupplyComponents = supplyComponents
                });
                MessageBox.Show("Привязка прошла успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
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
                supplyComponents.Remove(supplyComponents.Where(rec => rec.Value == (string)SelectedComponents.SelectedItem).ToList()[0].Key);
                LoadData();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (CanSelectedComponents.SelectedItems.Count == 1)
            {
                supplyComponents.Add((int)((ComponentViewModel)CanSelectedComponents.SelectedItem).Id
                    , ((ComponentViewModel)CanSelectedComponents.SelectedItem).ComponentName);
                LoadData();
            }
        }
    }
}
