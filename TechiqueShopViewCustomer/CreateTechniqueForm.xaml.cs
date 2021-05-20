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

namespace TechiqueShopViewCustomer
{
    /// <summary>
    /// Логика взаимодействия для CreateTechniqueForm.xaml
    /// </summary>
    public partial class CreateTechniqueForm : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public int CustomerId { set { customerId = value; } }

        public int Id { get { return (ComboBoxSupplyName.SelectedItem as SupplyViewModel).Id; } set { id = value; } }
        public DateTime Date { get { return (ComboBoxSupplyName.SelectedItem as SupplyViewModel).Date; } }

        private int? id;

        private int? customerId;

        private Dictionary<int, (string, int)> supplyGetTechiques;

        private readonly SupplyLogic logic;

        public CreateTechniqueForm(SupplyLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void CreateTechniqueForm_Loaded(object sender, RoutedEventArgs e)
        {
            var list = logic.Read(new SupplyBindingModel { CustomerId = customerId });
            
            if (list != null)
            {
                ComboBoxSupplyName.ItemsSource = list;
            }
            if (id != null)
            {
                ComboBoxSupplyName.SelectedItem = SetValue(id.Value);
            }
            TextBoxDate.Text = (DateTime.Now).ToString();
        }
        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxSupplyName.SelectedValue == null)
            {
                MessageBox.Show("Выберите поставку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var window = Container.Resolve<SelectOrderForm>();
            window.CustomerId = (int)customerId;
            if (window.ShowDialog().Value)
            {
                if (supplyGetTechiques.ContainsKey(window.Id))
                {
                    supplyGetTechiques[window.Id] = (window.OrderName, window.Count);
                }
                else
                {
                    supplyGetTechiques.Add(window.Id, (window.OrderName, window.Count));
                }
            }
            try
            {
                logic.CreateOrUpdate(new GetTechniqueBindingModel
                {
                    Id = id,
                    ArrivalTime = DateTime.Now,
                    SupplyGetTechniques = supplyGetTechiques,
                    CustomerId = customerId
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            DialogResult = true;
            Close();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
        private SupplyViewModel SetValue(int value)
        {
            foreach (var item in ComboBoxSupplyName.Items)
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
