using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atut.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public InvoiceType Type { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        public int JourneyId { get; set; }

        [Required]
        public virtual Journey Journey { get; set; }
    }

    public enum InvoiceType
    {
        PLN,
        Euro
    }
}
