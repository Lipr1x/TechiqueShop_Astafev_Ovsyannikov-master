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
    /// Логика взаимодействия для CreateAssemblyForm.xaml
    /// </summary>
    public partial class CreateAssemblyForm : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly AssemblyLogic assembly_logic;
        private readonly ComponentLogic component_logic;
        public int Id { set { id = value; } }
        private int? id;
        public int ProviderId { set { providerId = value; } }
        private int? providerId;
        private Dictionary<int, (string, int)> assemblyComponents;
        public CreateAssemblyForm(AssemblyLogic _assembly_logic, ComponentLogic _component_logic)
        {
            InitializeComponent();
            this.assembly_logic = _assembly_logic;
            this.component_logic = _component_logic;
        }
        private void CreateAssemblyForm_Loaded(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    AssemblyViewModel view = assembly_logic.Read(new AssemblyBindingModel { Id = id.Value })?[0];
                    if (view != null)
                    {
                        nameAssembly.Text = view.AssemblyName;
                        assemblyComponents = view.AssemblyComponents;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
                }
            }
            else
            {
                assemblyComponents = new Dictionary<int, (string, int)>();
            }
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                if (assemblyComponents != null)
                {
                    dataGrid.Columns.Clear();
                    var list = new List<DataGridAssemblyItemViewModel>();
                    foreach (var ac in assemblyComponents)
                    {
                        list.Add(new DataGridAssemblyItemViewModel()
                        {
                            Id = ac.Key,
                            AssemblyName = ac.Value.Item1,
                            Price = component_logic.Read(new ComponentBindingModel { Id = ac.Key })?[0].Price,
                            Count = ac.Value.Item2
                        });
                    }
                    dataGrid.ItemsSource = list;
                    dataGrid.Columns[0].Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (assemblyComponents == null || assemblyComponents.Count == 0)
            {
                MessageBox.Show("Заполните данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                assembly_logic.CreateOrUpdate(new AssemblyBindingModel
                {
                    Id = id,
                    AssemblyName = nameAssembly.Text,
                    Price = Convert.ToDecimal(totalCostAssembly.Text),
                    AssemblyComponents = assemblyComponents,
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

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<SelectComponentForm>();
            window.ProviderId = (int)providerId;
            if (window.ShowDialog().Value)
            {
                if (assemblyComponents.ContainsKey(window.Id))
                {
                    assemblyComponents[window.Id] = (window.OrderName, window.Count);
                }
                else
                {
                    assemblyComponents.Add(window.Id, (window.OrderName, window.Count));
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
                foreach (var so in assemblyComponents)
                {
                    totalCost += so.Value.Item2 * (int)component_logic.Read(new ComponentBindingModel { Id = so.Key })?[0].Price;
                }
                totalCostAssembly.Text = totalCost.ToString();
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
                var window = Container.Resolve<SelectComponentForm>();
                window.Id = ((DataGridSupplyItemViewModel)dataGrid.SelectedCells[0].Item).Id;
                window.Count = ((DataGridSupplyItemViewModel)dataGrid.SelectedCells[0].Item).Count;
                window.ProviderId = (int)providerId;

                if (window.ShowDialog().Value)
                {
                    assemblyComponents[window.Id] = (window.OrderName, window.Count);
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
                        assemblyComponents.Remove(((DataGridSupplyItemViewModel)dataGrid.SelectedCells[0].Item).Id);
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
