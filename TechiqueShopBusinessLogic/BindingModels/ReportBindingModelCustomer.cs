using System;
using System.Collections.Generic;
using System.Text;

namespace TechiqueShopBusinessLogic.BindingModels
{
    public class ReportBindingModelCustomer
    {
        public string FileName { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public int? CustomerId { get; set; }

        public List<string> deliverySupply { get; set; }
    }
}
