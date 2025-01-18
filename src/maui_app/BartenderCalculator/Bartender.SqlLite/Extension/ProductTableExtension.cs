using Bartender.SqlLite.Table;
using BartenderCalculator.Contracts.DTO;

namespace Bartender.SqlLite.Extension;

public static class ProductTableExtension
{
    public static ProductDto ToProductDto(this ProductTable productTable)
    {
        var dto = new ProductDto(productTable.Name, productTable.Price)
        {
            Id = productTable.Id
        };
        return dto;
    }
    
}