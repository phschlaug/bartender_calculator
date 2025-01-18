using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BartenderCalculator.Interface;
using Microsoft.Extensions.Logging;

namespace BartenderCalculator.ViewModels;

public class OrderViewModel: INotifyPropertyChanged
{
    public ObservableCollection<ProductViewModel> AvailableProducts { get; set; }
    public ObservableCollection<OrderItemViewModel> CurrentOrder { get; set; }
    
    public ICommand AddProductToOrderCommand { get; set; }
    public ICommand RemoveProductFromOrderCommand { get; set; }
    
    public ICommand ClearOrderCommand { get; set; }
    private decimal _orderPrice;
    private readonly ILogger<OrderViewModel> _logger;
    private readonly IOrderService _orderService;
    private readonly IProductService _productService;

    public OrderViewModel(IProductService productService, ILogger<OrderViewModel> logger, IOrderService orderService)
    {
        _logger = logger;
        _productService = productService;
        var availableProducts = _productService.GetAllProducts();
        _productService.ProductsUpdated += OnProductUpdate;
        AvailableProducts = new ObservableCollection<ProductViewModel>(availableProducts);
        CurrentOrder = [];
        AddProductToOrderCommand = new Command<ProductViewModel>(AddProductToOrder);
        RemoveProductFromOrderCommand = new Command<ProductViewModel>(RemoveProductFromOrder);
        ClearOrderCommand = new Command(ClearCurrentOrder);
        _orderService = orderService;
        OrderPrice = 0;
    }
    
    public decimal OrderPrice
    {
        get => _orderPrice;
        set
        {
            if (_orderPrice == value) return;
            _orderPrice = value;
            OnPropertyChanged();
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void AddProductToOrder(ProductViewModel product)
    {
        _orderService.AddProduct(product);
        RefreshOrder();
    }
   
    private void RemoveProductFromOrder(ProductViewModel product)
    {
        var currentOrderItem = CurrentOrder.FirstOrDefault(orderItem => orderItem.Id == product.Id);
        if (currentOrderItem != null)
        {
            _logger.LogDebug($"Remove all ${product.Name}");
            _orderService.RemoveProduct(currentOrderItem);
            RefreshOrder();
        }
    }

    private void ClearCurrentOrder()
    {
        _orderService.ClearOrder();
        RefreshOrder();
    }
    private void UpdateTotalPrice()
    {
        OrderPrice = _orderService.GetTotalPrice();
    }

    private void RefreshOrder()
    {
        CurrentOrder.Clear();
        foreach (var item in _orderService.GetCurrentOrder())
        {
            CurrentOrder.Add(item);
        }
        UpdateTotalPrice();
    }
    private void OnProductUpdate()
    {
        var availableProducts = _productService.GetAllProducts();
        AvailableProducts = new ObservableCollection<ProductViewModel>(availableProducts);
        _orderService.DatabaseUpdated(AvailableProducts);
        RefreshOrder();
        OnPropertyChanged(nameof(AvailableProducts));
    }

    public void OnStepperValueChanged(OrderItemViewModel orderItem, int quantity)
    {
        _logger.LogDebug("OnStepperValueChanged Method called");
        _logger.LogDebug($"Increase Quantity of {orderItem.Name} to {quantity}");
        var existingItem = CurrentOrder.FirstOrDefault(item => item.Id == orderItem.Id);
        if (existingItem == null) return;
        if (quantity == 0)
        {
            _logger.LogDebug($"Remove {existingItem.Name} from the order");
            _orderService.RemoveProduct(existingItem);
            RefreshOrder();
        }
        else
        {
            _logger.LogDebug($"Update quantity of product {existingItem.Name}");
            existingItem.Quantity = quantity;
        }
        UpdateTotalPrice();
    }
}