using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechiqueShopBusinessLogic.BindingModels
{
    public class AssemblyViewModel
    {
        public int? Id { get; set; }
        public int? ProviderId { get; set; }
        [DisplayName("Сборка")]
        public string AssemblyName { get; set; }
        [DisplayName("Цена")]
        public decimal Price { get; set; }

        public int? OrderId { get; set; }

        public Dictionary<int, (string, int)> AssemblyComponents { get; set; }
    }
}
