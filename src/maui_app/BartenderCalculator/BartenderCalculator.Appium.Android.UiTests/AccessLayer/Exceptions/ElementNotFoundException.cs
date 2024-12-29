namespace BartenderCalculator.Appium.Android.UiTests.AccessLayer.Exceptions;

public class ElementNotFoundException(string elementDescription)
    : Exception($"Could not find element {elementDescription}");