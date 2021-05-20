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
    /// Логика взаимодействия для SupplyForm.xaml
    /// </summary>
    public partial class SupplyForm : Window
    {
        
        [Dependency]
        public IUnityContainer Container { get; set; }
        public int Id { set { id = value; } }
        private int? id;

        private readonly SupplyLogic logic;
        public SupplyForm(SupplyLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void SupplyLogic_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var list = logic.Read(new SupplyBindingModel { CustomerId = id });
                if (list != null)
                {
                    dataGridView.ItemsSource = list;
                    dataGridView.Columns[1].Visibility = Visibility.Hidden;
                    //dataGridView.Columns[4].Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var window = Container.Resolve<CreateSupplyForm>();
            window.CustomerId = (int)id;
            if (window.ShowDialog().Value)
            {
                LoadData();
            }
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridView.SelectedItems.Count == 1)
            {
                var window = Container.Resolve<CreateSupplyForm>();
                window.Id = ((SupplyViewModel)dataGridView.SelectedItem).Id;
                window.CustomerId = (int)id;
                if (window.ShowDialog().Value)
                {
                    LoadData();
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridView.SelectedItems.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((SupplyViewModel)dataGridView.SelectedItem).Id;
                    try
                    {
                        logic.Delete(new SupplyBindingModel { Id = id });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    LoadData();
                }
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}
