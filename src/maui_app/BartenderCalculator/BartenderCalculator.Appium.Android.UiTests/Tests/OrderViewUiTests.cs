using BartenderCalculator.Appium.Android.UiTests.AccessLayer.BartenderApp.Views;
using BartenderCalculator.Appium.Android.UiTests.AccessLayer.DTO;
using FluentAssertions;

namespace BartenderCalculator.Appium.Android.UiTests.Tests;

[TestFixture]
public class OrderViewTests: BaseUiTests
{
    private OrderView _sut;

    private readonly ProductDto _beer = new ProductDto("Beer", 5);
    private readonly ProductDto _water = new ProductDto("Water", 2);
    
    [SetUp]
    public void SetUp()
    {
        BartenderCalculatorApp.Start();
        var configurationView = BartenderCalculatorApp.ConfigurationView;
        configurationView.AddProduct(_beer);
        configurationView.AddProduct(_water);
        _sut = BartenderCalculatorApp.OpenOrderView();
    }
    
    [Test]
    public void AddProductToOrder_AddingProductToOrder_ShouldUpdateTotalPriceAccordingly()
    {
        var expectedTotalPrice = _beer.Price;
        _sut.OrderProductWith(name: _beer.Name);

        var actualTotalPrice = _sut.GetTotalPrice();
        
        expectedTotalPrice.Should().Be(actualTotalPrice);
    }

    [Test]
    public void AddSameProductToOrder_AddingSameProductToOrder_ShouldUpdateTotalPriceAccordingly()
    {
         double expectedTotalPrice = _beer.Price * 2;
        _sut.OrderProductWith(name: _beer.Name);
        _sut.OrderProductWith(name: _beer.Name);
        var actualTotalPrice = _sut.GetTotalPrice();

        actualTotalPrice.Should().Be(expectedTotalPrice);
    }

    [Test]
    public void RemoveProductFromOrder_RemovingProductFromOrder_ShouldRemoveProductFromOrder()
    {
        _sut.OrderProductWith(name: _beer.Name);
        _sut.GetTotalPrice().Should().Be(_beer.Price);
        _sut.RemoveProductFromOrder(_beer.Name);
        _sut.GetTotalPrice().Should().Be(0);
    }

    [Test]
    public void ClearOrder_AddingBeerAndWaterClickingClearButton_ShouldHaveEmptyOrder()
    {
        _sut.OrderProductWith(name: _beer.Name);
        _sut.OrderProductWith(name: _water.Name);
        _sut.GetTotalPrice().Should().Be(_water.Price+_beer.Price);

        _sut.ClearOrder();
        
        _sut.GetTotalPrice().Should().Be(0);
    }

    [Test]
    public void StepperButton_AddingBeerToOrderIncreaseAndDecreaseViaStepper_ShouldUpdateLabelsAccordingly()
    {
        _sut.OrderProductWith(name: _beer.Name);
        _sut.IncreaseProductQuantity(_beer.Name);
        
        var actualAmount = _sut.GetAmountOf(_beer);
        actualAmount.Should().Be(2);
        
        _sut.DecreaseProductQuantity(_beer);

        actualAmount = _sut.GetAmountOf(_beer);
        actualAmount.Should().Be(1);
    }

    [Test]
    public void RemoveProductFromOrder_AddingBeerTwiceToOrderRemoveItViaTrashcanSymbol_ShouldBeAnEmptyOrder()
    {
        _sut.OrderProductWith(name: _beer.Name);
        _sut.OrderProductWith(name: _beer.Name);
        var totalPrice = _sut.GetTotalPrice();
        var totalPriceAfterAddingBeerTwice = _beer.Price * 2;
        totalPrice.Should().Be(totalPriceAfterAddingBeerTwice);
        
        _sut.RemoveProductFromOrder(_beer.Name);
        
        totalPrice = _sut.GetTotalPrice();
        
        totalPrice.Should().Be(0);
    }

    [TearDown]
    public void TearDown()
    {
        var configurationView = BartenderCalculatorApp.OpenConfigurationView();
        configurationView.RemoveAllProducts();
    }
    
}