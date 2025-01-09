using BartenderCalculator.Resources.Strings;

namespace BartenderCalculator.Views;

public partial class MobileMainView 
{
    public MobileMainView(ConfigurationView configurationView, OrderView orderView)
    {
        InitializeComponent();
        configurationView.AutomationId = "ConfigurationView";
        
        var configurationNavigationPage = new NavigationPage(configurationView)
        {
            Title = AppResources.ConfigurationView_TabText,
            AutomationId = "ConfigurationView"
        };
        SemanticProperties.SetDescription(configurationNavigationPage, 
            AppResources.MainView_SemanticDescription_ConfigView);
        var orderNavigationPage = new NavigationPage(orderView)
        {
            Title = AppResources.OrderView_TabText,
            AutomationId = "OrderView"
        };
        SemanticProperties.SetDescription(orderNavigationPage,
            AppResources.MainView_SemanticDescription_OrderView);
        
        Children.Add(configurationNavigationPage);
        Children.Add(orderNavigationPage);
    }
}