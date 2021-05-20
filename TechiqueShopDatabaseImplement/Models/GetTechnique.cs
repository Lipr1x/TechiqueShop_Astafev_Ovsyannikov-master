using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechiqueShopDatabaseImplement.Models
{
    public class GetTechnique // получение техники
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int SupplyId { get; set; }
        [Required]
        public DateTime ArrivalTime { get; set; }
        public virtual Supply Supply { get; set; }
        public virtual Customer Customer { get; set; }
    }
}