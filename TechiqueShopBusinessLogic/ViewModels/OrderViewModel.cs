using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechiqueShopBusinessLogic.BindingModels
{
    public class OrderViewModel
    {
        public int? Id { get; set; }
        [DisplayName("Заказ")]
        public string OrderName { get; set; }
        [DisplayName("Стоимость")]
        public decimal Price { get; set; }
    }
}
