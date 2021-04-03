using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TechiqueShopDatabaseImplement.Models
{
    public class Provider //поставщик
    {
        public int Id { get; set; }
        [Required]
        public string ProviderName { get; set; }
        [Required]
        public string ProviderSurname { get; set; }
        [Required]
        public string Patronymic { get; set; }
        [Required]
        public string Telephone { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string UserType { get; set; }
        [ForeignKey("ProviderId")]
        public virtual List<Assembly> Assemblies { get; set; }
        [ForeignKey("ProviderId")]
        public virtual List<Component> Components { get; set; }
        [ForeignKey("ProviderId")]
        public virtual List<Delivery> Deliveries { get; set; }
    }
}
