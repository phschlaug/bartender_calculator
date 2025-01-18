using BartenderCalculator.Contracts;
using BartenderCalculator.Contracts.DTO;
using BartenderCalculator.Interface;
using BartenderCalculator.ViewModels;

namespace BartenderCalculator.Services;

public class ProductService: IProductService
{
    private readonly IDatabase _databaseConnector;
    public ProductService(IDatabase databaseConnector)
    {
        _databaseConnector = databaseConnector;
    }

    public IEnumerable<ProductViewModel> GetAllProducts()
    {
        var productsInDatabase = _databaseConnector.GetProducts();
        return productsInDatabase.Select(productDto => new ProductViewModel(productDto.Id, productDto.Name, productDto.Price)).ToList();
    }

    public void UpdateProduct(ProductViewModel product)
    {
        var productToUpdate = MapToProductDto(product);
        _databaseConnector.Update(productToUpdate);
        ProductsUpdated?.Invoke();
    }

    public void SaveProduct(ProductViewModel product)
    {
        var productToSave = MapToProductDto(product);
        _databaseConnector.Insert(productToSave);
        ProductsUpdated?.Invoke();
    }

    public void DeleteProduct(ProductViewModel product)
    {
        var productToDelete = MapToProductDto(product);
        _databaseConnector.Delete(productToDelete);
        ProductsUpdated?.Invoke();
    }

    public event Action? ProductsUpdated;

    private ProductDto MapToProductDto(ProductViewModel productViewModel)
    {
        var productDto = new ProductDto(productViewModel.Name, productViewModel.Price);
        if (productViewModel.Id != 0)
        {
            productDto.Id = productViewModel.Id;
        }
        return productDto;
    }
}