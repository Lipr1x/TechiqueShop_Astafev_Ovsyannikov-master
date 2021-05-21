﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TechiqueShopBusinessLogic.BindingModels
{
    public class AssemblyBindingModel
    {
        public int? Id { get; set; }
        public int? ProviderId { get; set; }
        public int? OrderId { get; set; }
        public string AssemblyName { get; set; }
        public decimal Price { get; set; }
        public Dictionary<int, (string, int)> AssemblyComponents { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
