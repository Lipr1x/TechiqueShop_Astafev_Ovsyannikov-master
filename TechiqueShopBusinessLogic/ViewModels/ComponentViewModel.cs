using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechiqueShopBusinessLogic.BindingModels
{
    public class ComponentViewModel
    {
        public int? Id { get; set; }
        [DisplayName("Копмлектующая")]
        public string ComponentName { get; set; }
        [DisplayName("Цена")]
        public decimal Price { get; set; }
    }
}
