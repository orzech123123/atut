using System.Collections.Generic;

namespace Atut.ViewModels
{
    public class VatNumbersViewModel
    {
        public IEnumerable<VatNumberViewModel> VatNumbers { get; set; } = new List<VatNumberViewModel>();


        public KeyValueViewModel Company { get; set; }
    }

    public class VatNumberViewModel
    {
        public VatNumberViewModel()
        {
            MaxCharactersNumber = 100;
        }

        public string CountryName { get; set; }

        public string Number { get; set; }

        public int MaxCharactersNumber { get; set; } 
    }
}
