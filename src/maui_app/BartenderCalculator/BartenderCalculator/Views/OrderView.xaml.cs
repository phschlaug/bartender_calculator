using BartenderCalculator.Contracts.DTO;
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
        if (sender is not Stepper stepper) return;
        var orderItem = (OrderItemDto)stepper.BindingContext;
        var viewModel = (OrderViewModel)BindingContext;
        if (orderItem == null)
        {
            // Further investigation needed, this method gets called 
            _logger.LogDebug("OrderItem is null");
            return;
        }
        viewModel.OnStepperValueChanged(orderItem, (int)args.NewValue);
    }
}