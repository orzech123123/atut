using System.ComponentModel.DataAnnotations;

namespace Atut.Models
{
    public class Journey
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string StartingPlace { get; set; }
        
        public string UserId { get; set; }

        [Required]
        public virtual User User { get; set; }
    }
}
