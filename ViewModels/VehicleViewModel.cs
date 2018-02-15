﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Atut.ViewModels
{
    public class VehicleViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Numer rejestracyjny")]
        public string RegistrationNumber { get; set; }
    }
}
