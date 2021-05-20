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
        [DisplayName("Номер")]
        public int Id { get; set; }
        public int CustomerId { get; set; }

        [DisplayName("Время получения")]
        public DateTime? ArrivalTime { get; set; }
        public Dictionary<int, (string, int)> SupplyGetTechniques { get; set; }
    }
}
