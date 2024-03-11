using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppPizzeria.Models
{
    public class Utente
    {
        [Key]
        public int UtenteId { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [Required]
        [StringLength(255)]
        public string Nome { get; set; }

        [Required]
        [StringLength(255)]
        public string Cognome { get; set; }

        [Required]
        [Column(TypeName = "decimal(11, 0)")]
        public decimal Telefono { get; set; }

        [Required]
        [StringLength(50)]
        public string Ruolo { get; set; }

        public ICollection<OrdineProdotto> OrdiniProdotto { get; set; }

        public ICollection<RiepilogoOrdine> RiepilogoOrdini { get; set; }
    }
}