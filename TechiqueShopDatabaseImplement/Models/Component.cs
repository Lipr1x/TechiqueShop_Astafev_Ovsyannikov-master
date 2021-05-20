using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TechiqueShopDatabaseImplement.Models
{
    public class Component // комплектующие
    {
        public int Id { get; set; }
        public int ProviderId { get; set; }
        [Required]
        public string ComponentName { get; set; }
        [Required]
        public decimal Price { get; set; }
        [ForeignKey("ComponentId")]
        public virtual List<AssemblyComponent> AssemblyComponents { get; set; }
        [ForeignKey("ComponentId")]
        public virtual List<DeliveryComponent> DeliveryComponents { get; set; }
        public virtual Provider Provider { get; set; }
    }
}
