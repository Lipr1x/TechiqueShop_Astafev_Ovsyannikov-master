using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TechiqueShopDatabaseImplement.Models
{
    public class Customer // Заказчик
    {
        public int Id { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string CustomerSurname { get; set; }
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
        [ForeignKey("CustomerId")]
        public virtual List<Supply> Supplies { get; set; }
        [ForeignKey("CustomerId")]
        public virtual List<Order> Orders { get; set; }
        [ForeignKey("CustomerId")]
        public virtual List<GetTechnique> GetTechniques { get; set; }
    }
}
