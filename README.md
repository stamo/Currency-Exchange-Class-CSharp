Currency-Exchange-Class-CSharp
==============================

Description
----------

This is a C# implementation of my PHP Currency Exchange Class. It is a small but useful class which let you convert to and from several currencies, using the rates from European Central Bank

Constructor
------------

When called, constructor tries to connect to ECB server and download current currency rates. On success it will parse the XML file. Otherwise it will throw an Exception. Please, be advised that even when download is not possible and exception is thrown, the rate table will be populated with rates for BGN and EUR. The reason why that is done, is that Bulgarian Lev (BGN) is "strictly tied" to Euro (EUR) so it will always have value. First this class was intended to be used in Bulgaria, so even if we don't have full rates table, it will still provide some basic functionality for currency conversion between EUR and BGN.
Exceptions: if something goes wrong it throws WebException, FormatException or XmlException 

Public Properties
----------

`Date` 
You can check this property to get current rates date. Please, note that the reference rates are usually updated by 3 p.m. C.E.T. They are based on a regular daily concentration procedure between central banks across Europe and worldwide, which normally takes place at 2.15 p.m. CET. 

`BaseCurrency`
This property allows you to get and set the value of base currency. Base currency is used for displaying rates table and conversions. All calculations are performed according to base currency! Each time you set BaseCurrency, the rate table is recalculated.
Exceptions: Throws ApplicationException if value is not in currency list

Methods
-----------

There are several public methods that provide the main functionality of the class. They all use rates table that was created during the construction of the class. All rates in this table are calculated towards base currency (EUR by default). 

`ToString()`
Overrides original ToString() method. Creates string version of the rates table ready for printing. 

`public decimal Exchange(decimal amount, string from, string to = null)`
This method performs currency exchange on the given amount from one currency to the other. If destination currency is omitted, base currency is used instead. 

Parameters: `amount` The amount to be exchanged, `from` Currency of the amount (three letter code), `to` Currency to witch we wish to exchange. Base currency if not specified.

Return: the exchanged amount on success

Exceptions: Throws ApplicationException if currency code is not in currency list

`public decimal CrossRate(string from, string to = null)`
This method gets the cross rate between two currencies. If second currency is omitted, base currency is used instead. 

Parameters: `from` first Currency (three letter code), `to` second Currency (three letter code). Base currency if not specified.

Return: the cross rate on success

Exceptions: Throws ApplicationException if currency code is not in currency list

`public IEnumerable<Rates> GetRatesTable(string currencyList = null)` 
This method generates a replica of rates table based on Base currency. You can specify the list of currencies you need. You can use it for display purposes. 

Parameters: `currencyList ` comma separated list of Currencies to be included in the table. All currencies by default. In general you want need to show all currencies provided by ECB, so you can choose which to show by submitting a string list with currency codes.

Return: IEnumerable<Rates> which contains currency code as Rates.Currency and rate as Rates.Rate in format 0.0000. 
Exceptions: Throws ApplicationException if some of currency codes are not in currency list

`public IEnumerable<string> GetCurrencyList(bool sorted = false)`
This method gets the list of available currencies. If you are not sure which code to use, call this method and it will return a list of currency codes.

Parameters: `sorted` if this parameter is true, the list will be sorted. False by default.
Return: List of all available currencies 

Legal
------------

This class is completely open source. Feel free to modify it and use it as you desire. Please, keep in mind that I take no responsibility if something goes wrong. You use this class on your own risk! 
