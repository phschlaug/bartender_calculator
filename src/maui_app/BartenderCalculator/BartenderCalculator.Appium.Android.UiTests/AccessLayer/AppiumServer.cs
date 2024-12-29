using OpenQA.Selenium.Appium.Service;

namespace BartenderCalculator.Appium.Android.UiTests.AccessLayer;

public class AppiumServer
{
    private readonly AppiumLocalService _appiumLocalService;
    private const string ServerUrl = "127.0.0.1";
    private const int Port = 4723;

    public AppiumServer()
    {
        _appiumLocalService = new AppiumServiceBuilder()
            .WithIPAddress(ServerUrl)
            .UsingPort(Port)
            .Build();
    }

    public bool StartServer()
    {
        _appiumLocalService.Start();

        return _appiumLocalService.IsRunning;
    }

    public string GetServerUrl()
    {
        return $"http://{ServerUrl}:{Port}";
    }

    public bool StopServer()
    {
        if (_appiumLocalService.IsRunning)
        {
            _appiumLocalService.Dispose();
        }
        return _appiumLocalService.IsRunning;
    }
}