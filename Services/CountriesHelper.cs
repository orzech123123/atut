using System;
using System.Collections.Generic;
using System.Linq;
using Atut.Models;

namespace Atut.Services
{
    public class CountriesHelper
    {
        public static string Poland = "Polska [PL]";
        public static string Germany = "Niemcy [D]";
        public static string Denmark = "Dania [DK]";
        public static string Slovenia = "Słowenia [SI]";
        public static string Croatia = "Chorwacja [HR]";
        public static string Austria = "Austria [A]";
        public static string Belgium = "Belgia [B]";
        public static string Netherlands = "Holandia [NL]";
        
        public decimal GetTaxFactorForCountry(string country)
        {
            if (country == CountriesHelper.Poland)
            {
                return new decimal(1.23); //TODO czy tu na pewno tyle?
            }
            if (country == CountriesHelper.Germany)
            {
                return new decimal(1.19);
            }
            if (country == CountriesHelper.Slovenia)
            {
                return new decimal(1.095);
            }
            if (country == CountriesHelper.Austria)
            {
                return new decimal(1.1);
            }
            if (country == CountriesHelper.Belgium || country == CountriesHelper.Netherlands)
            {
                return new decimal(1.06);
            }
            if (country == CountriesHelper.Croatia)
            {
                return new decimal(1.25);
            }
            if (country == CountriesHelper.Denmark)
            {
                return new decimal(1.2);
            }

            throw new NotSupportedException($"Not supported tax for country: {country}");
        }

        public CurrencyType GetCurrencyForCountry(string country)
        {
            if (country == Poland)
            {
                return CurrencyType.PLN;
            }

            if (CountriesHelper.EuroCurrencyCountries.Contains(country))
            {
                return CurrencyType.EUR;
            }

            if (CountriesHelper.HrkCurrencyCountries.Contains(country))
            {
                return CurrencyType.HRK;
            }

            if (CountriesHelper.DkkCurrencyCountries.Contains(country))
            {
                return CurrencyType.DKK;
            }

            throw new NotSupportedException($"Not supported currency for country: {country}");
        }

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

        public static IDictionary<string, int> MaxVatNumberCharacters => new Dictionary<string, int>
        {
            { Poland, 50 }
        };
    }
}
