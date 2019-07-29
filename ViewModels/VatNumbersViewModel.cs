﻿using System.Collections.Generic;

namespace Atut.ViewModels
{
    public class VatNumbersViewModel
    {
        public IEnumerable<VatNumberViewModel> VatNumbers { get; set; } = new List<VatNumberViewModel>();


        public KeyValueViewModel Company { get; set; }
    }

    public class VatNumberViewModel
    {
        public string CountryName { get; set; }

        public string Number { get; set; }
    }
}
