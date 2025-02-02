using BartenderCalculator.Appium.Android.UiTests.AccessLayer.BartenderApp.Views;
using BartenderCalculator.Appium.Android.UiTests.AccessLayer.DTO;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;

namespace BartenderCalculator.Appium.Android.UiTests.AccessLayer.BartenderApp;

public class BartenderCalculatorApp
{
    // private const string OrderViewAutomationId = "OrderView";
    private const string orderViewXPath = "//android.widget.LinearLayout[@content-desc=\"Order\"]"; 
    
    // private const string ConfigurationViewAutomationId = "ConfigurationView";
    private const string configurationViewXPath = "//android.widget.LinearLayout[@content-desc=\"Configure Products\"]";
    
    private const string packageName = "com.phisch.bartendercalculator";
    private const string mainActivityName = ".mainactivity";
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
        driverOptions.AddAdditionalAppiumOption("appPackage", packageName);
        driverOptions.AddAdditionalAppiumOption("appActivity", mainActivityName);
        driverOptions.AddAdditionalAppiumOption("uiautomator2ServerLaunchTimeout",60000);
        driverOptions.AddAdditionalAppiumOption("noReset", true);

        _driver = new AndroidDriver(appiumServerUri, driverOptions, TimeSpan.FromSeconds(100));
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
    }

    public void Start()
    {
        _driver.StartActivity(packageName, mainActivityName);
        ConfigurationView = new ConfigurationView(_driver);
        OrderView = new OrderView(_driver);
    }

    public void Stop()
    {
        _driver.TerminateApp(packageName);
    }

    public ConfigurationView OpenConfigurationView()
    {
        var configurationView = _driver.FindElement(By.XPath(configurationViewXPath));
        configurationView.Click();
        return ConfigurationView;
    }

    public OrderView OpenOrderView()
    {
        var orderView = _driver.FindElement(By.XPath(orderViewXPath));
        orderView.Click();
        return OrderView;
    }

    public AlertView GetAlertView()
    {
        var alertView = new AlertView(_driver);
        return alertView;
    }

    public string TakeScreenshot(string filename)
    {
        var screenshotPath = $"{filename}.png";
        var screenshot = _driver.GetScreenshot();
        screenshot.SaveAsFile(screenshotPath);
        return screenshotPath;
    }

    public DeviceDto GetDeviceInfo()
    {
        var deviceName = _driver.Capabilities.GetCapability("deviceName");
        var platformName = _driver.Capabilities.GetCapability("platformName");
        var platformVersion = _driver.Capabilities.GetCapability("platformVersion");
        return new(deviceName.ToString(), platformName.ToString(), platformVersion.ToString());

    }
    
}