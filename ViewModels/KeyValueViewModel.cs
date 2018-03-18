using System.ComponentModel.DataAnnotations;

namespace Atut.ViewModels
{
    public class KeyValueViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
