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
        [DisplayName("Сборка")]
        public string AssemblyName { get; set; }
        [DisplayName("Цена")]
        public decimal Price { get; set; }
    }
}
