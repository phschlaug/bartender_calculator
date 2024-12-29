using BartenderCalculator.ViewModels;

namespace BartenderCalculator.Views;

public partial class ConfigurationView
{
    public ConfigurationView(ConfigurationViewModel viewModel)
    {
       InitializeComponent();
       BindingContext = viewModel;
    }
}