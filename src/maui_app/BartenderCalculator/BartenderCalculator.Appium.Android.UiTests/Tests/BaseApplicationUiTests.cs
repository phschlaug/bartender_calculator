using FluentAssertions;

namespace BartenderCalculator.Appium.Android.UiTests.Tests;

[TestFixture]
public class BaseApplicationUiTests: BaseUiTests
{
    [SetUp]
    public void Setup()
    {
        BartenderCalculatorApp.Start();
    }

    [Test]
    public void BasicNavigation_ToggleBetweenViews_ShouldShowEachView()
    {
        var configView = BartenderCalculatorApp.OpenConfigurationView();
        configView.IsVisible().Should().BeTrue();
        var orderView = BartenderCalculatorApp.OpenOrderView();
        orderView.IsVisible().Should().BeTrue();
    }
}