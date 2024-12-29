using BartenderCalculator.Contracts.DTO;
using FluentAssertions;

namespace BartenderCalculator.Contracts.UnitTests.DTO;

[TestFixture]
public class OrderItemDtoUnitTests
{
    [Test]
    public void TotalPrice_AddingTwoProductsWithPriceOfFive_ShouldHaveTotalPriceOfTen()
    {
        var productDto = new ProductDto{Name = "Dummy Product", Price = 5};
        var orderItemDto = new OrderItemDto(productDto, 2);
        
        var actualTotalPrice = orderItemDto.TotalPrice;
        
        actualTotalPrice.Should().Be(10);
    }
    
}