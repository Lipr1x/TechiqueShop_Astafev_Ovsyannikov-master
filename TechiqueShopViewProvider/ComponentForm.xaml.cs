using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private readonly TechiqueShopDatabase db = new TechiqueShopDatabase();
        public ComponentForm(ComponentLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                var list = logic.Read(null);
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
        private void ComponentForm_Closing(object sender, CancelEventArgs e)
        {
            db.Dispose();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            CreateComponentForm form = Container.Resolve<CreateComponentForm>();
            if (form.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (componentsGrid.SelectedItems.Count == 1)
            {
                CreateComponentForm form = Container.Resolve<CreateComponentForm>();
                form.Id = (int)((ComponentViewModel)componentsGrid.SelectedItem).Id;
                form.name.Text = ((ComponentViewModel)componentsGrid.SelectedItem).ComponentName;
                form.price.Text = ((ComponentViewModel)componentsGrid.SelectedItem).Price.ToString();
                if (form.ShowDialog() == true)
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
    }
}
