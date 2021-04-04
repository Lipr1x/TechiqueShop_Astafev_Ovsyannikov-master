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
        [DisplayName("Название поставки")]
        public string DeliveryName { get; set; }
        [DisplayName("Дата закупки")]
        public DateTime? Date { get; set; }
        public Dictionary<int, (string, int)> DeliveryComponents { get; set; }
    }
}
