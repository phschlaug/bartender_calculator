namespace BartenderCalculator.Appium.Android.UiTests.AccessLayer.Exceptions;

public class NotSupportedCurrencyException(string priceString) : Exception($"Unsupported Currency in '{priceString}'");