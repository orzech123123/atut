using System.Collections.Generic;

namespace Atut.Services
{
    public class CountriesProvider
    {
        public static string Poland = "Polska [PL]";
        public static string Germany = "Niemcy [D]";
        public static string Denmark = "Dania [DK]";
        public static string Slovenia = "Słowenia [SI]";
        public static string Croatia = "Chorwacja [HR]";
        public static string Austria = "Austria [A]";
        public static string Belgium = "Belgia [B]";
        public static string Netherlands = "Holandia [NL]";

        public static IEnumerable<string> Countries => new List<string>
        {
            Poland,
            Germany,
            Denmark,
            Slovenia,
            Croatia,
            Austria,
            Belgium,
            Netherlands
        };

        public static IEnumerable<string> EuroCurrencyCountries => new List<string>
        {
            Germany,
            Slovenia,
            Austria,
            Belgium,
            Netherlands
        };

        public static IEnumerable<string> HrkCurrencyCountries => new List<string>
        {
            Croatia
        };

        public static IEnumerable<string> DkkCurrencyCountries => new List<string>
        {
            Denmark
        };
    }
}
