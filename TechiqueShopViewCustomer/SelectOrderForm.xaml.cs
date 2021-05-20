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
    /// Логика взаимодействия для SelectOrderForm.xaml
    /// </summary>
    public partial class SelectOrderForm : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public int Id { get { return (ComboBoxOrderName.SelectedItem as OrderViewModel).Id; } set { id = value; } }
        public string OrderName { get { return (ComboBoxOrderName.SelectedItem as OrderViewModel).OrderName; } }
        public int Count { get { return Convert.ToInt32(TextBoxCount.Text); } set { TextBoxCount.Text = value.ToString(); } }
        public int CustomerId { set { customerId = value; } }
        private int? id;
        private int? customerId;

        private readonly OrderLogic logic;

        public SelectOrderForm(OrderLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void SelectOrderForm_Loaded(object sender, RoutedEventArgs e)
        {
            var list = logic.Read(new OrderBindingModel { CustomerId = customerId });
            if (list != null)
            {
                ComboBoxOrderName.ItemsSource = list;
            }
            if (id != null)
            {
                ComboBoxOrderName.SelectedItem = SetValue(id.Value);
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (ComboBoxOrderName.SelectedValue == null)
            {
                MessageBox.Show("Выберите косметику", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DialogResult = true;
            Close();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private OrderViewModel SetValue(int value)
        {
            foreach (var item in ComboBoxOrderName.Items)
            {
                if ((item as OrderViewModel).Id == value)
                {
                    return item as OrderViewModel;
                }
            }
            return null;
        }
    }
}
