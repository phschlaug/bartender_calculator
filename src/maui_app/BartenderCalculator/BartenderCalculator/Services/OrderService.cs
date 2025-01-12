using BartenderCalculator.Contracts.DTO;
using BartenderCalculator.Interface;
using BartenderCalculator.ViewModels;

namespace BartenderCalculator.Services;

public class OrderService: IOrderService
{
    private readonly List<OrderItemViewModel> _currentOrder = new();
    
    public IEnumerable<OrderItemViewModel> GetCurrentOrder()
    {
        return _currentOrder;
    }

    public void AddProduct(ProductDto product)
    {
        var existing = _currentOrder.FirstOrDefault(item => item.Product.Id == product.Id);
        if (existing != null)
        {
            existing.Quantity++;
        }
        else
        {
            _currentOrder.Add(new OrderItemViewModel(product));
        }
    }

    public void RemoveProduct(OrderItemViewModel orderItem)
    {
        var itemToRemove = _currentOrder.FirstOrDefault(item => item.Product.Id == orderItem.Product.Id);
        if (itemToRemove != null)
        {
            _currentOrder.Remove(itemToRemove);
        }
    }

    public void UpdateQuantity(OrderItemViewModel orderItem, int quantity)
    {
        var item = _currentOrder.FirstOrDefault(item => item.Product.Id == orderItem.Product.Id);
        if (item == null) return;
        if (quantity == 0)
        {
            _currentOrder.Remove(item);
        }
        else
        {
            item.Quantity = quantity;
        }
    }

    public void ClearOrder()
    {
        _currentOrder.Clear();
    }

    public decimal GetTotalPrice()
    {
        return _currentOrder.Sum(item => item.TotalPrice);
    }
}