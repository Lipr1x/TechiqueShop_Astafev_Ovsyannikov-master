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
        [DisplayName("ID прибытия")]
        public int Id { get; set; }
        public int CustomerId { get; set; }

        [DisplayName("Время получения")]
        public DateTime? ArrivalTime { get; set; }
        [DisplayName("ID поставки")]
        public int SupplyId { get; set; }
    }
}
