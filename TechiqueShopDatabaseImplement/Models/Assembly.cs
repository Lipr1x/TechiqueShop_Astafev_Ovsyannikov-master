using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TechiqueShopDatabaseImplement.Models
{
    public class Assembly //сборка
    {
        public int Id { get; set; }
        public int ProviderId { get; set; }
        [Required]
        public string AssemblyName { get; set; }
        [Required]
        public decimal Price { get; set; }
        [ForeignKey("AssemblyId")]
        public virtual List<AssemblyComponent> AssemblyComponents { get; set; }
        [ForeignKey("AssemblyId")]
        public virtual List<AssemblyOrder> AssemblyOrders { get; set; }
        public virtual Provider Provider { get; set; }
    }
}
