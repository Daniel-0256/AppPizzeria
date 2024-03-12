using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppPizzeria.Models
{
    public class OrderSummary
    {
        [Key]
        public int OrderSummaryId { get; set; }

        public string OrderDate { get; set; }

        public string OrderAddress { get; set; }

        public string Note { get; set; }

        public decimal TotalPrice { get; set; }

        public string State { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}