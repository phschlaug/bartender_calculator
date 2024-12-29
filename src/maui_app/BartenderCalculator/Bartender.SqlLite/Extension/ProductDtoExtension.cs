using Bartender.SqlLite.Table;
using BartenderCalculator.Contracts.DTO;

namespace Bartender.SqlLite.Extension;

public static class ProductDtoExtension
{
    public static ProductTable ToProduct(this ProductDto dto)
    {
        ProductTable productTable = new()
        {
            Id = dto.Id,
            Name = dto.Name,
            Price = dto.Price
        };
        return productTable;
    }
    
}