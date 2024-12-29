using BartenderCalculator.Resources.Strings;

namespace BartenderCalculator.Views;

public partial class MobileMainView 
{
    public MobileMainView(ConfigurationView configurationView, OrderView orderView)
    {
        InitializeComponent();
        configurationView.AutomationId = "ConfigurationView";
        
        Children.Add(new NavigationPage(configurationView)
        {
            Title = AppResources.ConfigurationView_TabText,
            AutomationId = "ConfigurationView"
        });
        Children.Add(new NavigationPage(orderView)
        {
            Title = AppResources.OrderView_TabText,
            AutomationId = "OrderView"
        });
    }
}