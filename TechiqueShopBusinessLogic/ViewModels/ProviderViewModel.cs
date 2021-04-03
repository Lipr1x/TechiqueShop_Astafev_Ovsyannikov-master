using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechiqueShopBusinessLogic.BindingModels
{
    public class ProviderViewModel
    {
        public int? Id { get; set; }
        [DisplayName("Имя")]
        public string ProviderName { get; set; }
        [DisplayName("Фамилия")]
        public string ProviderSurname { get; set; }
        [DisplayName("Отчество")]
        public string Patronymic { get; set; }
        [DisplayName("Телефон")]
        public string Telephone { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Пароль")]
        public string Password { get; set; }
        [DisplayName("Роль")]
        public string UserType { get; set; }
    }
}
