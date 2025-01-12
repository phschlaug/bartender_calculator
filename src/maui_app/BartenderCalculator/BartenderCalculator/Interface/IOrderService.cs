using BartenderCalculator.Contracts.DTO;
using BartenderCalculator.ViewModels;

namespace BartenderCalculator.Interface;

public interface IOrderService
{
    IEnumerable<OrderItemViewModel> GetCurrentOrder();
    void AddProduct(ProductDto product);
    void RemoveProduct(OrderItemViewModel orderItem);
    void UpdateQuantity(OrderItemViewModel orderItem, int quantity);
    void ClearOrder();
    decimal GetTotalPrice();
}