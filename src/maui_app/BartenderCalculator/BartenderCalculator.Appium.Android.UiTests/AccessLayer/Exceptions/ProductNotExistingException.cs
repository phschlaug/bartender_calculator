namespace BartenderCalculator.Appium.Android.UiTests.AccessLayer.Exceptions;

public class ProductNotExistingException(string productName) : Exception($"Product {productName} not exists");