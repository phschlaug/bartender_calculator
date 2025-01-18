using BartenderCalculator.ViewModels;

namespace BartenderCalculator.Interface;

public interface IProductService
{
    IEnumerable<ProductViewModel> GetAllProducts();
    void UpdateProduct(ProductViewModel product);
    void SaveProduct(ProductViewModel product);
    
    void DeleteProduct(ProductViewModel product);
    
    event Action ProductsUpdated;
}