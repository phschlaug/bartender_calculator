using AventStack.ExtentReports;
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
        TestReport.CreateNewTest("Basic Navigation Test", "Toggles between views");
        TestReport.LogInfo("Open the Navigation Page");
        var configView = BartenderCalculatorApp.OpenConfigurationView();
        var isConfigViewVisible = configView.IsVisible();
        if (isConfigViewVisible)
        {
            TestReport.LogInfo("ConfigView is visible");
        }
        else
        {
            TestReport.Fail( "ConfigView is not visible");
        }
        AttachScreenshot("ConfigurationViewVisible");
        isConfigViewVisible.Should().BeTrue();
        var orderView = BartenderCalculatorApp.OpenOrderView();
        TestReport.LogInfo("Open Order View");
        var isOrderViewVisible = orderView.IsVisible();
        if (isOrderViewVisible)
        {
            TestReport.LogInfo("OrderView is visible");
        }
        else
        {
            TestReport.Fail( "OrderView is not visible");
        }
        AttachScreenshot("OrderViewVisible");
        isOrderViewVisible.Should().BeTrue();
    }
}