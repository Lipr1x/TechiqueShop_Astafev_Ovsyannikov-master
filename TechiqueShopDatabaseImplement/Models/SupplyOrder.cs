using System.ComponentModel.DataAnnotations;

namespace TechiqueShopDatabaseImplement.Models
{
    public class SupplyOrder
    {
        public int Id { get; set; }
        public int SupplyId { get; set; }
        public int OrderId { get; set; }
        [Required]
        public decimal Sum { get; set; }
        public virtual Supply Supply { get; set; }
        public virtual Order Order { get; set; }
    }
}