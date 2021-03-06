﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Atut.Models
{
    public class Journey
    {
        public Journey()
        {
            Countries = new List<Country>();
            Invoices = new List<Invoice>();
            JourneyVehicles = new List<JourneyVehicle>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string StartingPlace { get; set; }
        
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

        public virtual ICollection<JourneyVehicle> JourneyVehicles { get; set; }

        public List<Invoice> Invoices { get; set; }

        [Required]
        public bool IsNotified { get; set; }
    }
}
