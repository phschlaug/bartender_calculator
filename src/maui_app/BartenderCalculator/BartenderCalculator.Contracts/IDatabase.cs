using System.Collections.Generic;
using BartenderCalculator.Contracts.DTO;

namespace BartenderCalculator.Contracts;

public interface IDatabase
{
    void Insert(ProductDto product);
    void Update(ProductDto product);
    void Delete(ProductDto product);
    bool ContainsProduct(ProductDto product);
    List<ProductDto> GetProducts();
    
    event Action ProductsUpdated;
}