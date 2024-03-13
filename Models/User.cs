using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Serialization;

namespace AppPizzeria.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Max 255 caratteri")]
        public string Email { get; set; }

        [Required]
        [StringLength(16, ErrorMessage = "Max 16 caratteri/numeri")]
        public string Password { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Max 30 caratteri")]
        public string Name { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Max 100 caratteri")]
        public string Surname { get; set; }

        [Required]
        public string Phone { get; set; }

        public string Role { get; set; } = "user";

        public ICollection<OrderSummary> OrderSummaries { get; set; }
    }
}