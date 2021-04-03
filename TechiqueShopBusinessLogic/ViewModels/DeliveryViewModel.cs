using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechiqueShopBusinessLogic.BindingModels
{
    public class DeliveryViewModel
    {
        public int? Id { get; set; }
        [DisplayName("Время закупки")]
        public DateTime? Date { get; set; }
        [DisplayName("Стоимость")]
        public decimal Price { get; set; }
    }
}
