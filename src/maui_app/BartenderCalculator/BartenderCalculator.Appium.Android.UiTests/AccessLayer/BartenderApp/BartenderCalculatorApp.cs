using BartenderCalculator.Appium.Android.UiTests.AccessLayer.BartenderApp.Views;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;

namespace BartenderCalculator.Appium.Android.UiTests.AccessLayer.BartenderApp;

public class BartenderCalculatorApp
{
    private const string OrderViewAutomationId = "OrderView";
    private const string OrderViewXPath = "//android.widget.TextView[@text=\"CREATE ORDER\"]";
    
    private const string ConfigurationViewAutomationId = "ConfigurationView";
    private const string ConfigurationViewXPath = "//android.widget.TextView[@text=\"CONFIGURE PRODUCTS\"]";
    
    private const string PackageName = "com.phisch.bartendercalculator";
    private const string MainActivityName = ".mainactivity";
    public ConfigurationView ConfigurationView { get; set; }
    private OrderView OrderView { get; set; }
    
    private readonly AndroidDriver _driver;

    public BartenderCalculatorApp(Uri appiumServerUri)
    {
        var driverOptions = new AppiumOptions{
            AutomationName = AutomationName.AndroidUIAutomator2,
            PlatformName = "Android",
            DeviceName = "Android Emulator",
        };
        driverOptions.AddAdditionalAppiumOption("appPackage", PackageName);
        driverOptions.AddAdditionalAppiumOption("appActivity", MainActivityName);
        driverOptions.AddAdditionalAppiumOption("uiautomator2ServerLaunchTimeout",6000 );
        driverOptions.AddAdditionalAppiumOption("noReset", true);

        _driver = new AndroidDriver(appiumServerUri, driverOptions, TimeSpan.FromSeconds(100));
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
    }

    public void Start()
    {
        _driver.StartActivity(PackageName, MainActivityName);
        ConfigurationView = new ConfigurationView(_driver);
        OrderView = new OrderView(_driver);
    }

    public void Stop()
    {
        //ToDo: Check how to close app
        _driver.Quit();
    }

    public ConfigurationView OpenConfigurationView()
    {
        var configurationView = _driver.FindElement(By.XPath(ConfigurationViewXPath));
        configurationView.Click();
        return ConfigurationView;
    }

    public OrderView OpenOrderView()
    {
        var orderView = _driver.FindElement(By.XPath(OrderViewXPath));
        orderView.Click();
        return OrderView;
    }

    public AlertView GetAlertView()
    {
        var alertView = new AlertView(_driver);
        return alertView;
    }
}