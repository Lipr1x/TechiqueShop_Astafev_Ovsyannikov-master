using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TechiqueShopBusinessLogic.BindingModels;
using TechiqueShopBusinessLogic.BusinessLogics;
using TechiqueShopDatabaseImplement;
using TechiqueShopDatabaseImplement.Models;
using Unity;

namespace TechiqueShopViewProvider
{
    /// <summary>
    /// Логика взаимодействия для ComponentForm.xaml
    /// </summary>
    public partial class ComponentForm : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly ComponentLogic logic;
        public int Id { set { id = value; } }
        private int? id;
        public ComponentForm(ComponentLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }
        private void ComponentForm_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                var list = logic.Read(new ComponentBindingModel { ProviderId = id });
                if (list != null)
                {
                    componentsGrid.ItemsSource = list;
                    componentsGrid.ColumnWidth = DataGridLength.Auto;
                    //componentsGrid.Columns[1].Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
            }
        }
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<CreateComponentForm>();
            window.ProviderId = (int)id;
            if (window.ShowDialog().Value)
            {
                LoadData();
            }
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (componentsGrid.SelectedItems.Count == 1)
            {
                var window = Container.Resolve<CreateComponentForm>();
                window.Id = ((OrderViewModel)componentsGrid.SelectedItem).Id;
                window.name.Text = ((ComponentViewModel)componentsGrid.SelectedItem).ComponentName;
                window.price.Text = ((ComponentViewModel)componentsGrid.SelectedItem).Price.ToString();
                window.ProviderId = (int)id;
                if (window.ShowDialog().Value)
                {
                    LoadData();
                }
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (componentsGrid.SelectedItems.Count == 1)
            {
                if (MessageBox.Show("Удалить компонент?", "Вопрос", MessageBoxButton.YesNo,
               MessageBoxImage.Question) == MessageBoxResult.Yes)
                { 
                    try
                    {
                        int id = Convert.ToInt32(((ComponentViewModel)componentsGrid.SelectedItem).Id);
                        logic.Delete(new ComponentBindingModel { Id = id });
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
