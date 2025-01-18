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
        var productDto = new ProductDto("Test Product", 50)
        {
            Id = 1,
        };

        var actualProduct = productDto.ToProduct();

        actualProduct.Id.Should().Be(productDto.Id);
        actualProduct.Name.Should().Be(productDto.Name);
        actualProduct.Price.Should().Be(productDto.Price);
    }
    
}