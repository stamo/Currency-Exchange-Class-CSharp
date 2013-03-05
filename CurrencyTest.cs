using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverter
{
    class CurrencyTest
    {
        static void Main()
        {
            Currency ratetable = new Currency();
            ratetable.BaseCurrency = "BGN";

            Console.WriteLine("----------- ToString() Test -----------\n");
            Console.WriteLine(ratetable);
            Console.WriteLine();
            Console.WriteLine("-------- GetCurrencyList() Test -------\n");
            String[] CurrencyList;
            CurrencyList = ratetable.GetCurrencyList().ToArray();
            foreach (var item in CurrencyList)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
            Console.WriteLine(" Exchange(5M, \"EUR \", \"usd\") Test \n");
            Console.WriteLine(ratetable.Exchange(5M, "EUR ", "usd"));
            Console.WriteLine();
            Console.WriteLine("-------- CrossRate(\"EUR\") Test -------\n");
            Console.WriteLine(ratetable.CrossRate("EUR"));
            Console.WriteLine();
            Console.WriteLine("GetRatesTable(\"eur, bgn; usd,gbp CHF  \") Test\n");
            IEnumerable<Rates> customRates;
            customRates = ratetable.GetRatesTable("eur, bgn; usd,gbp CHF  ");
            foreach (var rate in customRates)
            {
                Console.WriteLine("{0} = {1}", rate.Currency, rate.Rate);
            }
        }
    }
}
