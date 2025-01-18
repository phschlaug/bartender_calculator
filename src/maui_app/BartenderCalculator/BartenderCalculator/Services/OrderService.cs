using BartenderCalculator.Interface;
using BartenderCalculator.ViewModels;

namespace BartenderCalculator.Services;

public class OrderService: IOrderService
{
    private List<OrderItemViewModel> _currentOrder = new();
    
    public IEnumerable<OrderItemViewModel> GetCurrentOrder()
    {
        return _currentOrder;
    }

    public void AddProduct(ProductViewModel product)
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

    public void DatabaseUpdated(IList<ProductViewModel> products)
    {
        foreach (var currentOrderProduct in _currentOrder)
        {
            foreach (var product in products)
            {
                if (currentOrderProduct.Product.Id != product.Id) continue;
                currentOrderProduct.Product.Name = product.Name;
                currentOrderProduct.Product.Price = product.Price;
            }
        }
        _currentOrder = _currentOrder.Where(item => products.Any(product => product.Id == item.Id)).ToList();
    }
}