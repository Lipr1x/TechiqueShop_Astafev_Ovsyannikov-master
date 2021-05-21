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
        [Dependency]
        public IUnityContainer Container { get; set; }

        public int ComponentId { get { return (int)(ComboBoxComponent.SelectedItem as ComponentViewModel).Id; } }

        public int SupplyId { get { return (ComboBoxSupply.SelectedItem as SupplyViewModel).Id; } set { supplyId = value; } }

        public int CustomerId { set { customerId = value; } }

        private int? supplyId;

        private int? customerId;

        private readonly ComponentLogic logicC;

        private readonly SupplyLogic logicS;

        public LinkingSupplyForm(ComponentLogic logicC, SupplyLogic logicS)
        {
            InitializeComponent();
            this.logicC = logicC;
            this.logicS = logicS;
        }

        private void LinkingSupplyForm_Loaded(object sender, RoutedEventArgs e)
        {
            var listVisit = logicC.Read(null);
            if (listVisit != null)
            {
                ComboBoxComponent.ItemsSource = listVisit;
            }
            var listSupply = logicS.Read(new SupplyBindingModel { CustomerId = customerId });
            if (listSupply != null)
            {
                ComboBoxSupply.ItemsSource = listSupply;
            }
            if (supplyId != null)
            {
                ComboBoxSupply.SelectedItem = SetValueDistribution(supplyId.Value);
            }
        }

        private void buttonLinking_Click(object sender, RoutedEventArgs e)
        {

            if (ComboBoxComponent.SelectedValue == null)
            {
                MessageBox.Show("Выберите компонент", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (ComboBoxSupply.SelectedValue == null)
            {
                MessageBox.Show("Выберите поставку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                logicS.Linking(new SupplyLinkingBindingModel
                {
                    ComponentId = (int)(ComboBoxComponent.SelectedItem as ComponentViewModel).Id,
                    SupplyId = (int)(ComboBoxSupply.SelectedValue as SupplyViewModel).Id
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

        private SupplyViewModel SetValueDistribution(int value)
        {
            foreach (var item in ComboBoxSupply.Items)
            {
                if ((item as SupplyViewModel).Id == value)
                {
                    return item as SupplyViewModel;
                }
            }
            return null;
        }
    }
}
