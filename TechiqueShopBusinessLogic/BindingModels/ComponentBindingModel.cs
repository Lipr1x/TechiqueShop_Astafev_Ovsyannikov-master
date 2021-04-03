using System;
using System.Collections.Generic;
using System.Text;

namespace TechiqueShopBusinessLogic.BindingModels
{
    public class ComponentBindingModel
    {
        public int? Id { get; set; }
        public string ComponentName { get; set; }
        public decimal Price { get; set; }
    }
}
