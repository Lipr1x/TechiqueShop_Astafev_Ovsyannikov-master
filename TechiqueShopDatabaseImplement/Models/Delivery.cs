using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TechiqueShopDatabaseImplement.Models
{
    public class Delivery // закупка
    {
        public int Id { get; set; }
        public int ProviderId { get; set; }
        [Required]
        public string DeliveryName { get; set; }
        [Required]
        public DateTime? Date { get; set; }
        [Required]
        public decimal Price { get; set; }
        [ForeignKey("DeliveryId")]
        public virtual List<DeliveryComponent> DeliveryComponents { get; set; }
        public virtual Provider Provider { get; set; }
    }
}
