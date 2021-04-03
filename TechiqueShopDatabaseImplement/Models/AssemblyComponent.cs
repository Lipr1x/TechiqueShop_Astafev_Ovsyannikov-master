using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TechiqueShopDatabaseImplement.Models
{
    public class AssemblyComponent
    {
        public int Id { get; set; }
        public int AssemblyId { get; set; }
        public int ComponentId { get; set; }
        [Required]
        public int Count { get; set; }
        public virtual Component Component { get; set; }
        public virtual Assembly Assembly { get; set; }
    }
}
