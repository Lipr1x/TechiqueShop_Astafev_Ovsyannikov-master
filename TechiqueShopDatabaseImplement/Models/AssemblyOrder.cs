using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TechiqueShopDatabaseImplement.Models
{
    public class AssemblyOrder
    {
        public int Id { get; set; }
        public int AssemblyId { get; set; }
        public int OrderId { get; set; }
        public virtual Assembly Assembly { get; set; }
        public virtual Order Order { get; set; }
    }
}
