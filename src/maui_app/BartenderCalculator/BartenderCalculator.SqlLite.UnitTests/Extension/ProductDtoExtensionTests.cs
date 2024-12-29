using Bartender.SqlLite.Extension;
using BartenderCalculator.Contracts.DTO;
using FluentAssertions;

namespace BartenderCalculator.SqlLite.UnitTests.Extension;

[TestFixture]
public class ProductDtoExtensionTests
{
    [Test]
    public void ToProduct_UsingASimpleProductDto_ShouldMapAllRelevantProperties()
    {
        var productDto = new ProductDto
        {
            Id = 1,
            Name = "Test Product",
            Price = 50
        };

        var actualProduct = productDto.ToProduct();

        actualProduct.Id.Should().Be(productDto.Id);
        actualProduct.Name.Should().Be(productDto.Name);
        actualProduct.Price.Should().Be(productDto.Price);
    }
    
}