using BartenderCalculator.Views;
using Microsoft.Maui.Devices;
namespace BartenderCalculator;

public partial class App : Application
{
    private ConfigurationView _configurationView;
    private OrderView _orderView;
    
    public App(ConfigurationView configurationView, OrderView orderView)
    {
        InitializeComponent();
        _configurationView = configurationView;
        _orderView = orderView;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        Page mainPage = DeviceInfo.Platform.ToString() switch
        {
            "Android" or "iOS" => new MobileMainView(_configurationView, _orderView),
            "MacCatalyst" or "WinUi" => new DesktopMainView(_configurationView, _orderView),
            _ => new ContentPage { Content = new Label { Text = "Platform not supported" }} 
        };
        return new Window(mainPage);
    }
}