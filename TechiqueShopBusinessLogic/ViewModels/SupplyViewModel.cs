using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechiqueShopBusinessLogic.BindingModels
{
    public class SupplyViewModel
    {
        public int Id { get; set; }
        [DisplayName("Поставка")]
        public string SupplyName { get; set; }

        [DisplayName("Дата поставки")]
        public DateTime? Date { get; set; }
        public Dictionary<int, (string, int)> SupplyOrders { get; set; }
    }
}
