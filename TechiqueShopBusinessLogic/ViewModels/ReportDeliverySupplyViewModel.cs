using System;
using System.Collections.Generic;
using System.Text;

namespace TechiqueShopBusinessLogic.ViewModels
{
    public class ReportDeliverySupplyViewModel
    {
        public string DeliveryName { get; set; }

        public decimal Price { get; set; }

        public DateTime Date { get; set; }

        public int Count { get; set; }

        public int ClientId { get; set; }

        public decimal TotalCost { get; set; }
    }
}
