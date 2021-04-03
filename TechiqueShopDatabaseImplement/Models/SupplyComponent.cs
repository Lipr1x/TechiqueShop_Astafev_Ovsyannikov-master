using System.ComponentModel.DataAnnotations;

namespace TechiqueShopDatabaseImplement.Models
{
    public class SupplyComponent
    {
        public int Id { get; set; }
        public int SupplyId { get; set; }
        public int ComponentId { get; set; }
        public virtual Component Component { get; set; }
        public virtual Supply Supply { get; set; }
    }
}