using System;
using System.Collections.Generic;
using System.Text;

namespace TechiqueShopBusinessLogic.BindingModels
{
    public class SupplyBindingModel
    {
        public int? Id { get; set; }
        public string SupplyName { get; set; }
        public DateTime Date { get; set; }
        public Dictionary<int, (string, int)> SupplyOrders { get; set; }
    }
}
