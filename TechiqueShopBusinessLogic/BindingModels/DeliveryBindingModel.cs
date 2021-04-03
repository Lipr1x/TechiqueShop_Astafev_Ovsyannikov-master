using System;
using System.Collections.Generic;
using System.Text;

namespace TechiqueShopBusinessLogic.BindingModels
{
    public class DeliveryBindingModel
    {
        public int? Id { get; set; }
        public DateTime? Date { get; set; }
        public decimal Price { get; set; }
    }
}
