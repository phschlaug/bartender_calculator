using BartenderCalculator.ViewModels;
using Microsoft.Extensions.Logging;

namespace BartenderCalculator.Views;

public partial class OrderView
{
    private readonly ILogger<OrderView> _logger;
    public OrderView(OrderViewModel viewModel, ILogger<OrderView> logger)
    {
        _logger = logger;
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void OnStepperValueChanged(object sender, ValueChangedEventArgs args)
    {
        _logger.LogDebug($"Old value {args.OldValue} New value {args.NewValue}");
        if (sender is Stepper { BindingContext: OrderItemViewModel orderItem } )
        {
            var newQuantity = orderItem.Quantity;
            if (BindingContext is OrderViewModel viewModel)
            {
                viewModel.OnStepperValueChanged(orderItem, newQuantity);
            }
        }
    }
}