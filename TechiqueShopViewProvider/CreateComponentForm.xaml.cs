using System;
using System.Windows;
using TechiqueShopBusinessLogic.BindingModels;
using TechiqueShopBusinessLogic.BusinessLogics;
using Unity;

namespace TechiqueShopViewProvider
{
    /// <summary>
    /// Логика взаимодействия для CreateComponentForm.xaml
    /// </summary>
    public partial class CreateComponentForm : Window
    {
        [Dependency]
        public IUnityContainer Container { get; set; }
        private readonly ComponentLogic logic;
        public int Id { set => id = value; }
        private int? id;
        public int ProviderId { set { providerId = value; } }
        private int? providerId;

        public CreateComponentForm(ComponentLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }
        private void CreateComponentForm_Loaded(object sender, RoutedEventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var view = logic.Read(new ComponentBindingModel { Id = id })?[0];
                    if (view != null)
                    {
                        name.Text = view.ComponentName;
                        price.Text = view.Price.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка");
                }
            }
        }
        private void Save_button(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(name.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка");
                return;
            }
            try
            {
                logic.CreateOrUpdate(new ComponentBindingModel
                {
                    Id = id,
                    ComponentName = name.Text,
                    Price = Convert.ToInt32(price.Text),
                    ProviderId = providerId
                });
                this.DialogResult = true;
                MessageBox.Show("Сохранение прошло успешно", "Сообщение");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }
        private void Close_button(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
