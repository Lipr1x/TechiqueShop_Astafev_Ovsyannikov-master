using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TechiqueShopDatabaseImplement.Models
{
    public class DeliveryComponent
    {
        public int Id { get; set; }
        public int DeliveryId { get; set; }
        public int ComponentId { get; set; }
        [Required]
        public int Count { get; set; }
        public virtual Component Component { get; set; }
        public virtual Delivery Delivery { get; set; }
    }
}
