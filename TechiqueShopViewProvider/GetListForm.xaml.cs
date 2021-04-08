using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
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
    /// Логика взаимодействия для GetListForm.xaml
    /// </summary>
    public partial class GetListForm : Window
    {
        private readonly GetListLogic list_logic;
        private readonly ComponentLogic comp_logic;
        private Dictionary<int, string> Components;
        public GetListForm(GetListLogic list_logic, ComponentLogic comp_logic)
        {
            InitializeComponent();
            this.list_logic = list_logic;
            this.comp_logic = comp_logic;
            this.Loaded += GetListForm_Loaded;
        }
        private void LoadData()
        {
            try
            {
                if (Components != null)
                {
                    CanSelectedMedicationsListBox.Items.Clear();
                    foreach (var comp in comp_logic.Read(null))
                    {
                        CanSelectedMedicationsListBox.Items.Add(comp.ComponentName);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
               MessageBoxImage.Error);
            }
        }

        private void WordButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new SaveFileDialog { Filter = "docx|*.docx" })
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    List<String> selected_medications = new List<string>();
                    foreach (var item in CanSelectedMedicationsListBox.SelectedItems)
                    {
                        selected_medications.Add(item.ToString());
                    }
                    list_logic.SaveComponentsToWordFile(new GetListBindingModel { FileName = dialog.FileName, Components = selected_medications });
                    System.Windows.MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK,
               MessageBoxImage.Information);
                }
            }
        }

        private void ExcelButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new SaveFileDialog { Filter = "xlsx|*.xlsx" })
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    List<String> selected_medications = new List<string>();
                    foreach (var item in CanSelectedMedicationsListBox.SelectedItems)
                    {
                        selected_medications.Add(item.ToString());
                    }
                    list_logic.SaveDishComponentToExcelFile(new GetListBindingModel { FileName = dialog.FileName, Components = selected_medications });
                    System.Windows.MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK,
               MessageBoxImage.Information);
                }
            }
        }

        private void CancelButton__Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void GetListForm_Loaded(object sender, RoutedEventArgs e)
        {
            Components = new Dictionary<int, string>();
            LoadData();
        }
    }
}
