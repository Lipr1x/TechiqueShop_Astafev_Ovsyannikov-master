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

namespace TechiqueShopViewProvider
{
    /// <summary>
    /// Логика взаимодействия для WindowLinkingAssembly.xaml
    /// </summary>
    public partial class WindowLinkingAssembly : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public int OrderId { get { return (int)(ComboBoxOrder.SelectedItem as OrderViewModel).Id; } }

        public int AssemblyId { get { return (int)(ComboBoxAssembly.SelectedItem as AssemblyViewModel).Id; } set { assemblyId = value; } }

        public int ProviderId { set { providerId = value; } }

        private int? assemblyId;

        private int? providerId;

        private readonly OrderLogic logicOrder;

        private readonly AssemblyLogic logicAssembly;

        public WindowLinkingAssembly(OrderLogic logicOrder, AssemblyLogic logicAssembly)
        {
            InitializeComponent();
            this.logicAssembly = logicAssembly;
            this.logicOrder = logicOrder;
        }

        private void WindowLinkingAssembly_Loaded(object sender, RoutedEventArgs e)
        {
            var listOrder = logicOrder.Read(null);
            if (listOrder != null)
            {
                ComboBoxOrder.ItemsSource = listOrder;
            }
            var listAssembly = logicAssembly.Read(new AssemblyBindingModel { ProviderId = providerId });
            if (listAssembly != null)
            {
                ComboBoxAssembly.ItemsSource = listAssembly;
            }
            if (assemblyId != null)
            {
                ComboBoxAssembly.SelectedItem = SetValueAssembly(assemblyId.Value);
            }
        }

        private void buttonLinking_Click(object sender, RoutedEventArgs e)
        {

            if (ComboBoxOrder.SelectedValue == null)
            {
                MessageBox.Show("Выберите заказ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (ComboBoxAssembly.SelectedValue == null)
            {
                MessageBox.Show("Выберите сборку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                logicAssembly.Linking(new AssemblyLinkingBindingModel
                {
                    OrderId = (ComboBoxOrder.SelectedItem as OrderViewModel).Id,
                    AssemblyId = (int)(ComboBoxAssembly.SelectedItem as AssemblyViewModel).Id
                }); 
                MessageBox.Show("Привязка прошла успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private AssemblyViewModel SetValueAssembly(int value)
        {
            foreach (var item in ComboBoxAssembly.Items)
            {
                if ((item as AssemblyViewModel).Id == value)
                {
                    return item as AssemblyViewModel;
                }
            }
            return null;
        }
    }
}