using BartenderCalculator.Resources.Strings;

namespace BartenderCalculator.Views
{
    public partial class DesktopMainView : FlyoutPage
    {
        private readonly ConfigurationView _configurationView;
        private readonly OrderView _orderView;
        
        public DesktopMainView(ConfigurationView configurationView, OrderView orderView)
        {
            InitializeComponent();
            _configurationView = configurationView;
            _orderView = orderView;
        }

        private void ShowConfiguration(object sender, EventArgs e)
        {
            Detail = new NavigationPage(_configurationView);
            IsPresented = false; // Closes the Flyout menu
        }

        private void ShowOrder(object sender, EventArgs e)
        {
            Detail = new NavigationPage(_orderView);
            IsPresented = false; // Closes the Flyout menu
        }
    }
}