using System;
using System.Collections.Generic;
using System.Text;

namespace TechiqueShopBusinessLogic.BindingModels
{
    public class ReportBindingModelProvider
    {
        public string FileName { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public int? ProviderId { get; set; }
    }
}
