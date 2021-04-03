using System;
using System.Collections.Generic;
using System.Text;

namespace TechiqueShopBusinessLogic.BindingModels
{
    public class AssemblyBindingModel
    {
        public int? Id { get; set; }
        public string AssemblyName { get; set; }
        public decimal Price { get; set; }
        public Dictionary<int, (string, int)> AssemblyComponents { get; set; }
    }
}
