using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Atut.Models;

namespace Atut.Services
{
    public static class ExchangeCache
    {
        private static readonly IDictionary<string, decimal> Cache = new Dictionary<string, decimal>();

        public static void CheckCache(DateTime date, CurrencyType currency, decimal value)
        {
            var key = $"{date}{currency}";

            if (Cache.ContainsKey(key))
            {
                var cacheValue = Cache[key];

                if (cacheValue != value)
                {
                    throw new InvalidOperationException($"Kurs dla {currency} z dnia {date} jest sprzeczny: {cacheValue} != {value}");
                }

                return;
            }

            Cache.Add(key, value);
        }
    }
}
