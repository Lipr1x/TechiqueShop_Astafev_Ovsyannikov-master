using System;
using System.Collections.Generic;
using System.Text;

namespace TechiqueShopBusinessLogic.BindingModels
{
    public class OrderBindingModel
    {
        public int? Id { get; set; }
        public int? CustomerId { get; set; }
        public string OrderName { get; set; }
        public decimal Price { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo{ get; set; }

        public Dictionary<int, (string, int)> OrderSupplies { get; set; }
    }
}
