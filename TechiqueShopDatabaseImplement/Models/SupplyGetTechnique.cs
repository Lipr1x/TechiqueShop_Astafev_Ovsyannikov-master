using System.ComponentModel.DataAnnotations;

namespace TechiqueShopDatabaseImplement.Models
{
    public class SupplyGetTechnique
    {
        public int Id { get; set; }
        public int SupplyId { get; set; }
        public int GetTechniqueId { get; set; }
        public virtual GetTechnique GetTechnique { get; set; }
        public virtual Supply Supply { get; set; }
    }
}