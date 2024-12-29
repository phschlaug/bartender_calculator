using BartenderCalculator.Appium.Android.UiTests.AccessLayer;
using BartenderCalculator.Appium.Android.UiTests.AccessLayer.BartenderApp;

namespace BartenderCalculator.Appium.Android.UiTests.Tests;

public class BaseUiTests
{
    private AppiumServer _appiumServer;
    protected BartenderCalculatorApp BartenderCalculatorApp;
    
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _appiumServer = new AppiumServer();
        var isRunning = _appiumServer.StartServer();
        if (!isRunning) throw new ApplicationException("Failed to start server");
        var appiumServerUri = new Uri(_appiumServer.GetServerUrl());
        BartenderCalculatorApp = new BartenderCalculatorApp(appiumServerUri);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _appiumServer.StopServer();
    }
}