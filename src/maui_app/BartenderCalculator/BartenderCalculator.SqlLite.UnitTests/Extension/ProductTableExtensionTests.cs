using Bartender.SqlLite.Extension;
using Bartender.SqlLite.Table;
using FluentAssertions;

namespace BartenderCalculator.SqlLite.UnitTests.Extension;

public class ProductTableExtensionTests
{
    [Test]
    public void ToProductTable_WithValidProductTableInstance_ShouldReturnProductDtoContainingAllProperties()
    {
        var productTable = new ProductTable
        {
            Id = 1,
            Name = "Test Product",
            Price = 10m,
        };
        var productDto = productTable.ToProductDto();
        productDto.Id.Should().Be(productTable.Id);
        productDto.Name.Should().Be(productTable.Name);
        productDto.Price.Should().Be(productTable.Price);
    }
}