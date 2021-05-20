using System;
using System.Collections.Generic;
using System.Text;

namespace TechiqueShopBusinessLogic.BindingModels
{
    public class SupplyBindingModel
    {
        public int? Id { get; set; }
        public int? CustomerId { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime Date { get; set; }
        public Dictionary<int, (string, int)> SupplyOrders { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
