using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Atut.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string CompanyNameShort { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
