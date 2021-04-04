using System;
using System.Collections.Generic;
using System.Text;

namespace TechiqueShopBusinessLogic.BindingModels
{
    public class GetTechniqueBindingModel
    {
        public int? Id { get; set; }
        public string GetTechniqueName { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public Dictionary<int, (string, int)> SupplyGetTechniques { get; set; }
    }
}
