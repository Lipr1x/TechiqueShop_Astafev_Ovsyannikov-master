using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechiqueShopBusinessLogic.BindingModels
{
    public class GetTechniqueViewModel
    {
        public int? Id { get; set; }
        [DisplayName("Время получения")]
        public DateTime? ArrivalTime { get; set; }
    }
}
