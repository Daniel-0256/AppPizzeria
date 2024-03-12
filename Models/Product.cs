using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppPizzeria.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Max 255 caratteri")]
        public string ProductName { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Max 1000 caratteri")]
        public string ProductPhone { get; set; }

        [Required]
        [Range(1, 99, ErrorMessage = "Scegli un prezzo da 1€ a 99€")]
        public decimal ProductPrice { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Max 50 caratteri")]
        public string PreparationTime { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Max 1000 caratteri")]
        public string Ingredients { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Devi scegliere una categoria!")]
        public string Category { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}