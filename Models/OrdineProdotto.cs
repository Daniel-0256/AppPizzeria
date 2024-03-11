using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppPizzeria.Models
{
    public class OrdineProdotto
    {
        [Key]
        public int OrdineProdottoId { get; set; }

        public int OrdineId { get; set; }

        public int ProdottoId { get; set; }

        public int UtenteId { get; set; }

        [Required]
        public int Quantità { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PrezzoOrdine { get; set; }

        public Prodotto Prodotto { get; set; }

        public Utente Utente { get; set; }

        public RiepilogoOrdine RiepilogoOrdine { get; set; }
    }
}