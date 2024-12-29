using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BartenderCalculator.Contracts;
using BartenderCalculator.Contracts.DTO;
using Microsoft.Extensions.Logging;

namespace BartenderCalculator.ViewModels;

public class OrderViewModel: INotifyPropertyChanged
{
    public ObservableCollection<ProductDto> AvailableProducts { get; set; }
    public ObservableCollection<OrderItemDto> CurrentOrder { get; set; }
    
    public ICommand AddProductToOrderCommand { get; set; }
    public ICommand RemoveProductFromOrderCommand { get; set; }
    
    public ICommand ClearOrderCommand { get; set; }
    private decimal _orderPrice;
    private readonly IDatabase _database;
    private readonly ILogger<OrderViewModel> _logger;

    public OrderViewModel(IDatabase database, ILogger<OrderViewModel> logger)
    {
        _logger = logger;
        _database = database;
        var availableProducts = _database.GetProducts();
        _database.ProductsUpdated += OnProductUpdate;
        AvailableProducts = new ObservableCollection<ProductDto>(availableProducts);
        CurrentOrder = [];
        AddProductToOrderCommand = new Command<ProductDto>(AddProductToOrder);
        RemoveProductFromOrderCommand = new Command<ProductDto>(RemoveProductFromOrder);
        ClearOrderCommand = new Command(ClearCurrentOrder);
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

    private void OnCollectionItemChanged(OrderItemDto orderItem)
    {
        var index = CurrentOrder.IndexOf(orderItem);
        CurrentOrder[index] = orderItem;
    }
    private void AddProductToOrder(ProductDto product)
    {
        var currentOrderItem = CurrentOrder.FirstOrDefault(orderItem => orderItem.Product.Id == product.Id);
        if (currentOrderItem != null)
        {
            _logger.LogDebug($"Increase Quantity of {product.Name}");
            currentOrderItem.Quantity++;
            OnCollectionItemChanged(currentOrderItem);
        }
        else
        {
            _logger.LogDebug($"Add Product to {product.Name}");
            CurrentOrder.Add(new OrderItemDto(product));
        }
        UpdateTotalPrice();
    }
   
    private void RemoveProductFromOrder(ProductDto product)
    {
        var currentOrderItem = CurrentOrder.FirstOrDefault(orderItem => orderItem.Product.Id == product.Id);
        if (currentOrderItem != null)
        {
            _logger.LogDebug($"Remove all ${product.Name}");
            CurrentOrder.Remove(currentOrderItem);
        }
        UpdateTotalPrice();
    }

    private void ClearCurrentOrder()
    {
        CurrentOrder.Clear();
        UpdateTotalPrice();
    }
    private void UpdateTotalPrice()
    {
        OrderPrice = CurrentOrder.Sum(orderItem => orderItem.TotalPrice);
    }
    private void OnProductUpdate()
    {
        var availableProducts = _database.GetProducts();
        AvailableProducts = new ObservableCollection<ProductDto>(availableProducts);
        OnPropertyChanged(nameof(AvailableProducts));
    }

    public void OnStepperValueChanged(OrderItemDto orderItem, int quantity)
    {
        _logger.LogDebug($"Increase Quantity of {orderItem.Product.Name} to {quantity}");
        if (quantity == 0)
        {
            CurrentOrder.Remove(orderItem);
        }
        else
        {
            orderItem.Quantity = quantity;
        }
        UpdateTotalPrice();
    }

}