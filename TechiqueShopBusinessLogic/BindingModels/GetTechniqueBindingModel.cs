using System;
using System.Collections.Generic;
using System.Text;

namespace TechiqueShopBusinessLogic.BindingModels
{
    public class GetTechniqueBindingModel
    {
        public int? Id { get; set; }
        public int? CustomerId { get; set; }
        //public string GetTechniqueName { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int SupplyId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
