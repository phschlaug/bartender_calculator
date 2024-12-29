using System.Globalization;
using BartenderCalculator.Appium.Android.UiTests.AccessLayer.Exceptions;

namespace BartenderCalculator.Appium.Android.UiTests.AccessLayer.Helper;

public static class StringParser
{
    private static readonly Dictionary<string, CultureInfo> CurrencyCultureInfoDictionary = new Dictionary<string, CultureInfo>
    {
        { "$", CultureInfo.GetCultureInfo("en-US") },
        { "€", CultureInfo.GetCultureInfo("de-DE") },
        { "£", new CultureInfo("en-GB")}
    };
    public static double ParsePrice(string priceString)
    {
        foreach (var currencyKey in CurrencyCultureInfoDictionary.Keys)
        {
            if (priceString.Contains(currencyKey))
            {
                priceString = priceString.Replace(currencyKey, string.Empty);
                priceString = priceString.Trim();
                return double.Parse(priceString, CurrencyCultureInfoDictionary[currencyKey]);
            }
        }
        throw new NotSupportedCurrencyException(priceString);
    }
}