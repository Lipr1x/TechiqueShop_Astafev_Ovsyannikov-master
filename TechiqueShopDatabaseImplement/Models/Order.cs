using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TechiqueShopDatabaseImplement.Models
{
    public class Order // заказ
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        [Required]
        public string OrderName { get; set; }
        [Required]
        public decimal Price { get; set; }
        [ForeignKey("OrderId")]
        public virtual List<SupplyOrder> SupplyOrders { get; set; }
        [ForeignKey("OrderId")]
        public virtual List<AssemblyOrder> AssemblyOrders { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
