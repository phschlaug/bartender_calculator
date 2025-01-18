using BartenderCalculator.Contracts.DTO;
using BartenderCalculator.ViewModels;

namespace BartenderCalculator.Interface;

public interface IOrderService
{
    IEnumerable<OrderItemViewModel> GetCurrentOrder();
    void AddProduct(ProductViewModel product);
    void RemoveProduct(OrderItemViewModel orderItem);
    void UpdateQuantity(OrderItemViewModel orderItem, int quantity);
    void ClearOrder();
    decimal GetTotalPrice();
    
    void DatabaseUpdated(IList<ProductViewModel> products);
}