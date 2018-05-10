using System.ComponentModel.DataAnnotations;

namespace Atut.ViewModels
{
    public class KeyValueViewModel
    {
        [Required]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
