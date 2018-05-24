using System.Collections.Generic;

namespace Atut.Services
{
    public class CountriesProvider
    {
        public static IEnumerable<string> Countries => new List<string>
        {
            "Polska [PL]", "Niemcy [D]", "Dania [DK]", "Słowenia [SI]", "Chorwacja [HR]", "Austria [A]", "Belgia [B]", "Holandia [NL]"
        };
    }
}
