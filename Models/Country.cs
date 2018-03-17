using System;
using System.ComponentModel.DataAnnotations;

namespace Atut.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Distance { get; set; }
        
        public int JourneyId { get; set; }

        [Required]
        public virtual Journey Journey { get; set; }
    }
}
