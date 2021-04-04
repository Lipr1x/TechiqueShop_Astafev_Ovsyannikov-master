﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TechiqueShopBusinessLogic.BindingModels
{
    public class DeliveryBindingModel
    {
        public int? Id { get; set; }
        public string DeliveryName { get; set; }
        public DateTime? Date { get; set; }
        public Dictionary<int, (string, int)> DeliveryComponents { get; set; }
    }
}
