using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Globalization;
using System.Xml;

namespace Converter
{
    public class Currency
    {
        private string sourceUrl = @"http://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml";
        private XmlTextReader xml;
        private string baseCurrency;
        private Dictionary<string, decimal> exchangeRates = new Dictionary<string,decimal>();
        private DateTime date = new DateTime();
        private string currency;
        private decimal rate;

        public DateTime Date
        {
            get
            {
                return this.date;
            }
        }

        public string BaseCurrency
        {
            get
            {
                return this.baseCurrency;
            }
            set
            {
                if (value == null)
                {
                    value = "EUR";
                }
                CheckCurrency(value);
                this.baseCurrency = value;
                decimal factor = this.exchangeRates[this.baseCurrency];
                foreach (KeyValuePair<string, decimal> kvp in this.exchangeRates)
                {
                    this.exchangeRates[kvp.Key] /= factor; 
                }
            }
        }

        public Currency()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            try
            {
                xml = new XmlTextReader(sourceUrl);
            }
            catch (WebException we)
            {
                this.baseCurrency = "EUR";
                this.exchangeRates.Add(this.baseCurrency, 1M);
                this.exchangeRates.Add("BGN", 1.9558M);
                this.date = DateTime.Now;
                throw new WebException("Error downloading XML, exchange rate created for BGN only, base currency EUR!", we);
            }
            try
            {
                while (xml.Read())
                {
                    if (xml.Name == "Cube")
                    {
                        if (xml.AttributeCount == 1)
                        {
                            xml.MoveToAttribute("time");
                            this.date = DateTime.Parse(xml.Value);
                        }
                        if (xml.AttributeCount == 2)
                        {
                            xml.MoveToAttribute("currency");
                            this.currency = xml.Value;
                            xml.MoveToAttribute("rate");
                            try
                            {
                                this.rate = decimal.Parse(xml.Value);
                            }
                            catch (FormatException fe)
                            {
                                throw new FormatException("Urecognised rate format!", fe);
                            }
                            this.exchangeRates.Add(currency, rate);
                        }
                        xml.MoveToNextAttribute();
                    }
                }
            }
            catch (XmlException xe)
            {
                throw new XmlException("Unable to parse Euro foreign exchange reference rates XML!", xe);
            }
            this.baseCurrency = "EUR";
            this.exchangeRates.Add(this.baseCurrency, 1M);
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append("Reference rates of European Central Bank\nAll rates are for 1 " + this.baseCurrency + "\n\n");
            foreach (KeyValuePair<string, decimal> kvp in this.exchangeRates)
            {
                str.Append(String.Format("{0}{1,15:0.0000}\n", kvp.Key, kvp.Value));
            }
            return str.ToString();
        }

        private void CheckCurrency(string currency)
        {
            if (!this.exchangeRates.ContainsKey(currency))
            {
                throw new ApplicationException("Unknown currency '" + currency + "', please use GetCurrencyList() to get list of available currencies!", new KeyNotFoundException());
            }
        }

        public decimal Exchange(decimal ammount, string from, string to = null)
        {
            decimal result = 0M;
            if (to == null)
            {
                to = this.baseCurrency;
            }
            CheckCurrency(from);
            CheckCurrency(to);
            result = ammount * this.exchangeRates[to] / this.exchangeRates[from];
            return result;
        }

        public decimal CrossRate(string from, string to = null)
        {
            decimal result = 0M;
            if (to == null)
            {
                to = this.baseCurrency;
            }
            CheckCurrency(from);
            CheckCurrency(to);
            result = this.exchangeRates[to] / this.exchangeRates[from];
            return result;
        }

        public Dictionary<string, decimal> GetRatesTable(List<string> currencyList = null)
        {
            if (currencyList == null)
            {
                return this.exchangeRates;
            }
            else 
            {
                Dictionary<string, decimal> result = new Dictionary<string, decimal>();
                foreach (string currency in currencyList)
                {
                    CheckCurrency(currency);
                    result.Add(currency, this.exchangeRates[currency]);
                }
                return result;
            }
        }

        public List<string> GetCurrencyList()
        {
            List<string> currencyList = new List<string>();
            foreach (KeyValuePair<string, decimal> kvp in this.exchangeRates)
            {
                currencyList.Add(kvp.Key);
            }
            return currencyList;
        }
    }
}