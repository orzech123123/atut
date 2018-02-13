using System.ComponentModel.DataAnnotations;

namespace Atut.Models
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string RegistrationNumber { get; set; }
    }
}
