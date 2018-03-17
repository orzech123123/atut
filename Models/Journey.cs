using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Atut.Models
{
    public class Journey
    {
        public Journey()
        {
            Countries = new List<Country>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string StartingPlace { get; set; }

        [Required]
        public string ThroughPlace { get; set; }

        [Required]
        public string FinalPlace { get; set; }

        [Required]
        public int AmountOfPeople { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
        
        public string UserId { get; set; }

        [Required]
        public virtual User User { get; set; }

        public List<Country> Countries { get; set; }

        [Required]
        public int TotalDistance { get; set; }

        [Required]
        public int OtherCountriesTotalDistance { get; set; }
    }
}
