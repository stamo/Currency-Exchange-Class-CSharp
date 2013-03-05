using System;
using System.Collections.Generic;

namespace CurrencyConverter
{
    interface ICurrencyConverter
    {
        //-----------------Properties--------------------------

        //Use this readonly property to check the actual date for the rates

        DateTime Date { get; }

        //Use this property to get or set base currency
        //Base currency is used for displaying rates table and convertions. All calculations are performed according to base currency!
        //EUR by default
        //Throws ApplicationException if value is not in currency list

        string BaseCurrency { get; set; } 

        //-----------------Methods------------------------------

        //Exchanges the given amount from one currency to the other
        //param Decimal amount The amount to be exchanged
        //param String from Currency of the amount (three letter code)
        //param String to Currency to witch we wish to exchange. Base currency if not specified.
        //returns Decimal - the exchanged amount on success
        //Throws ApplicationException if currency is not in currency list

        decimal Exchange(decimal amount, string from, string to = null);

        //Gets the cross rate between two currencies
        //param String from first Currency (three letter code)
        //param String to second Currency (three letter code). Base currency if not specified.
        //returns decimal - the cross rate on success
        //Throws ApplicationException if currency is not in currency list

        decimal CrossRate(string from, string to = null);

        //Gets the rates table based on Base currency
        //param string currencyList - list of comma separated Currencies to be included in the table. All currencies by default
        //returns IEnumerable<KeyValuePair<string, string>> containing desired currencies and rates
        //Throws ApplicationException if currency is not in currency list

        IEnumerable<KeyValuePair<string, string>> GetRatesTable(string currencyList = null);

        //Gets the list of currencies. If sorted is true, the returned list is sorted. False by default
        //returns IEnumerable<string> of all available currencies 

        IEnumerable<string> GetCurrencyList(bool sorted = false);
    }
}
