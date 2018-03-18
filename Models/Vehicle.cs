using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atut.Models
{
    public class Vehicle
    {
        public Vehicle()
        {
            JourneyVehicles = new List<JourneyVehicle>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string RegistrationNumber { get; set; }
        
        public string UserId { get; set; }

        [Required]
        public virtual User User { get; set; }
        
        public virtual ICollection<JourneyVehicle> JourneyVehicles { get; set; }
    }
}
