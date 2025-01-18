using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;

namespace BartenderCalculator.Appium.Android.UiTests.AccessLayer.BartenderApp.Views;

internal class AlertViewPath
{
    public readonly string TitleXPath = "//android.widget.TextView[@resource-id=\"com.phisch.bartendercalculator:id/alertTitle\"]";
    public readonly string Message = "android:id/message";
    public readonly string Button = "android:id/button2";
}

public class AlertView: IView
{
    private readonly AlertViewPath _path;
    private readonly AndroidDriver _driver;

    public AlertView(AndroidDriver driver)
    {
        _driver = driver;
        _path = new AlertViewPath();
    }

    public void CloseAlertView()
    {
        var okButton = _driver.FindElement(By.Id(_path.Button));
        okButton.Click();
    }

    public string GetTitle()
    {
        var titleText = _driver.FindElement(By.XPath(_path.TitleXPath));
        return titleText.Text;
    }

    public string GetMessage()
    {
        var message = _driver.FindElement(By.Id(_path.Message));
        return message.Text;
    }
    
    public bool IsVisible()
    {
        var title = _driver.FindElement(By.XPath(_path.TitleXPath));
        return title.Displayed;
    }
}