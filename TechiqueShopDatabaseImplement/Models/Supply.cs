using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TechiqueShopDatabaseImplement.Models
{
    public class Supply // поставка
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        [Required]
        public int SupplyName { get; set; }
        public DateTime? Date { get; set; }
        [ForeignKey("SupplyId")]
        public virtual List<SupplyComponent> SupplyComponents { get; set; }
        [ForeignKey("SupplyId")]
        public virtual List<SupplyOrder> SupplyOrders { get; set; }
        [ForeignKey("SupplyId")]
        public virtual List<SupplyGetTechnique> SupplyGetTechniques { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
