using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppPizzeria.Models
{
    public class Prodotto
    {
        [Key]
        public int ProdottoId { get; set; }

        [Required]
        [StringLength(255)]
        public string NomeProdotto { get; set; }

        [Required]
        [StringLength(255)]
        public string FotoProdotto { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PrezzoProdotto { get; set; }

        [Required]
        [StringLength(255)]
        public string TempoDiPreparazione { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        public string Ingredienti { get; set; }

        [Required]
        [StringLength(255)]
        public string Categoria { get; set; }

        public OrdineProdotto OrdineProdotto { get; set; }
    }
}