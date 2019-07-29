using System;
using System.ComponentModel.DataAnnotations;

namespace Atut.Models
{
    public class VatNumber
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CountryName { get; set; }

        [Required]
        public string Number { get; set; }

        [Required]
        public virtual User User { get; set; }
    }
}
