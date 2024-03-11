using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppPizzeria.Models
{
    public class RiepilogoOrdine
    {
        [Key]
        public int OrdineId { get; set; }

        public int UtenteId { get; set; }

        public DateTime DataOrdine { get; set; }

        [StringLength(255)]
        public string IndirizzoSpedizione { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Nota { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal PrezzoTotale { get; set; }

        [StringLength(50)]
        public string Stato { get; set; }

        public ICollection<OrdineProdotto> OrdiniProdotto { get; set; }

        public Utente Utente { get; set; }
    }
}