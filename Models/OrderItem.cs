using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppPizzeria.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        public int OrderSummaryId { get; set; }

        public int ProductId { get; set; }

        public int UserId { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "Min. 1, Max. 10")]
        public int Quantity { get; set; }

        public decimal ItemPrice { get; set; }

        public OrderSummary OrderSummary { get; set; }
        public User User { get; set; }
        public Product Product { get; set; }
    }
}