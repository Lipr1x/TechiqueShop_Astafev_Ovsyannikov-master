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
    /// Логика взаимодействия для CreateAssemblyForm.xaml
    /// </summary>
    public partial class CreateAssemblyForm : Window
    {
        private readonly AssemblyLogic assembly_logic;
        private readonly ComponentLogic component_logic;
        public int Id { set { id = value; } }
        private int? id;
        private Dictionary<int, string> assemblyComponents;
        public CreateAssemblyForm(AssemblyLogic _assembly_logic, ComponentLogic _component_logic)
        {
            InitializeComponent();
            this.assembly_logic = _assembly_logic;
            this.component_logic = _component_logic;
            this.Loaded += CreateAssemblyForm_Loaded;
        }
        private void CreateAssemblyForm_Loaded(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    AssemblyViewModel view = assembly_logic.Read(new AssemblyBindingModel
                    {
                        Id = id.Value
                    })?[0];
                    assemblyComponents = view.AssemblyComponents;
                    nameAssembly.Text = view.AssemblyName.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
                }
            }
            else
            {
                assemblyComponents = new Dictionary<int, string>();
            }
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                if (assemblyComponents != null)
                {
                    SelectedComponents.Items.Clear();
                    //dataGridView.Columns[0].Visible = false;
                    foreach (var comp in assemblyComponents)
                    {
                        SelectedComponents.Items.Add(comp.Value);
                    }
                }
                CanSelectedComponents.Items.Clear();
                foreach (var m in component_logic.Read(null))
                {
                    if (assemblyComponents.Values.Where(rec => rec == m.ComponentName).ToList().Count == 0) { 
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
            if (string.IsNullOrEmpty(nameAssembly.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
                return;
            }
            if (assemblyComponents == null || assemblyComponents.Count == 0)
            {
                MessageBox.Show("Заполните компоненты", "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
                return;
            }
            try
            {
                assembly_logic.CreateOrUpdate(new AssemblyBindingModel
                {
                    Id = id,
                    AssemblyName = nameAssembly.Text,
                    Price = Convert.ToInt32(priceAssembly.Text),
                    AssemblyComponents = assemblyComponents,
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
                assemblyComponents.Remove(assemblyComponents.Where(rec => rec.Value == (string)SelectedComponents.SelectedItem).ToList()[0].Key);
                LoadData();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (CanSelectedComponents.SelectedItems.Count == 1)
            {
                assemblyComponents.Add((int)((ComponentViewModel)CanSelectedComponents.SelectedItem).Id
                    , ((ComponentViewModel)CanSelectedComponents.SelectedItem).ComponentName);
                LoadData();
            }
        }
    }
}
