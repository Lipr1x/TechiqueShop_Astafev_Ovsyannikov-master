using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TechiqueShopBusinessLogic.BindingModels;
using TechiqueShopBusinessLogic.BusinessLogics;
using Unity;

namespace TechiqueShopViewProvider
{
    /// <summary>
    /// Логика взаимодействия для SelectComponentForm.xaml
    /// </summary>
    public partial class SelectComponentForm : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        public int Id { get { return (ComboBoxOrderName.SelectedItem as ComponentViewModel).Id; } set { id = value; } }
        public string OrderName { get { return (ComboBoxOrderName.SelectedItem as ComponentViewModel).ComponentName; } }
        public int Count { get { return Convert.ToInt32(TextBoxCount.Text); } set { TextBoxCount.Text = value.ToString(); } }
        public int ProviderId { set { providerId = value; } }
        private int? id;
        private int? providerId;

        private readonly ComponentLogic logic;

        public SelectComponentForm(ComponentLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void SelectComponentForm_Loaded(object sender, RoutedEventArgs e)
        {
            var list = logic.Read(new ComponentBindingModel { ProviderId = providerId });
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

        private ComponentViewModel SetValue(int value)
        {
            foreach (var item in ComboBoxOrderName.Items)
            {
                if ((item as ComponentViewModel).Id == value)
                {
                    return item as ComponentViewModel;
                }
            }
            return null;
        }
    }
}
