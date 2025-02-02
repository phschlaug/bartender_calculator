using AventStack.ExtentReports.Model;
using BartenderCalculator.Appium.Android.UiTests.AccessLayer.BartenderApp.Views;
using BartenderCalculator.Appium.Android.UiTests.AccessLayer.DTO;
using FluentAssertions;

namespace BartenderCalculator.Appium.Android.UiTests.Tests;

[TestFixture]
public class OrderViewTests: BaseUiTests
{
    private OrderView _sut;

    private readonly ProductDto _beer = new ("Beer", 5);
    private readonly ProductDto _water = new ("Water", 2);
    private readonly ProductDto _soda = new ("Soda", 1);
    
    [SetUp]
    public void SetUp()
    {
        BartenderCalculatorApp.Start();
        var configurationView = BartenderCalculatorApp.ConfigurationView;
        configurationView.AddProduct(_beer);
        configurationView.AddProduct(_water);
        configurationView.AddProduct(_soda);
        _sut = BartenderCalculatorApp.OpenOrderView();
    }
    
    [Test]
    public void AddProductToOrder_AddingProductToOrder_ShouldUpdateTotalPriceAccordingly()
    {
        TestReport.CreateNewTest("Add Product To Order", $"Adding {_beer} to the order");
        var expectedTotalPrice = _beer.Price;
        TestReport.LogInfo($"Adding {_beer} to current order");
        _sut.OrderProductWith(name: _beer.Name);

        var actualTotalPrice = _sut.GetTotalPrice();
        expectedTotalPrice.Should().Be(actualTotalPrice);
        TestReport.Pass($"The total price of the is {actualTotalPrice} as expected");
        AttachScreenshot("AddOneProductToOrder", "Verify one product was added to the order");
    }

    [Test]
    public void AddSameProductToOrder_AddingSameProductToOrder_ShouldUpdateTotalPriceAccordingly()
    {
        TestReport.CreateNewTest("Add Product To Order", $"Adding {_beer} twice to the order");
         var expectedTotalPrice = _beer.Price * 2;
         TestReport.LogInfo($"Adding {_beer} twice to current order");
        _sut.OrderProductWith(name: _beer.Name);
        _sut.OrderProductWith(name: _beer.Name);
        
        var actualTotalPrice = _sut.GetTotalPrice();
        actualTotalPrice.Should().Be(expectedTotalPrice);
        TestReport.Pass($"The total price of the is {actualTotalPrice} as expected");
    }

    [Test]
    public void RemoveProductFromOrder_RemovingProductFromOrder_ShouldRemoveProductFromOrder()
    {
        TestReport.CreateNewTest("Remove Product from Order", 
            $"Adding {_beer} to the order, and remove it again");
        TestReport.LogInfo($"Adding {_beer} to current order");
        _sut.OrderProductWith(name: _beer.Name);
        _sut.GetTotalPrice().Should().Be(_beer.Price);
                
        TestReport.LogInfo($"Removing {_beer} from the current order");
        _sut.RemoveProductFromOrder(_beer.Name);
        
        _sut.GetTotalPrice().Should().Be(0);
        TestReport.Pass("The total price is at 0, as expected");
    }

    [Test]
    public void ClearOrder_AddingBeerAndWaterClickingClearButton_ShouldHaveEmptyOrder()
    {
        TestReport.CreateNewTest("Clear Order", $"Adding {_beer} and {_water} to order and clear all");
        TestReport.LogInfo($"Adding {_beer} and {_water} to current order");
        _sut.OrderProductWith(name: _beer.Name);
        _sut.OrderProductWith(name: _water.Name);
        var expectedSubTotalPrice = _beer.Price + _water.Price;
        _sut.GetTotalPrice().Should().Be(expectedSubTotalPrice);
        TestReport.Pass($"The current sub total price is {expectedSubTotalPrice} as expected");

        TestReport.LogInfo("Clear order via button");
        _sut.ClearOrder();
        
        _sut.GetTotalPrice().Should().Be(0);
        TestReport.Pass("The total price is at 0, as expected");
    }

    [Test]
    public void StepperButton_AddingBeerToOrderIncreaseAndDecreaseViaStepper_ShouldUpdateLabelsAccordingly()
    {
        TestReport.CreateNewTest("Stepper Button", 
            "Increase/Decrease quantity of order item via stepper button");
        TestReport.LogInfo($"Adding {_beer} to current order");
        _sut.OrderProductWith(name: _beer.Name);
        TestReport.LogInfo($"Increase quantity of {_beer} via stepper button");
        _sut.IncreaseProductQuantity(_beer.Name);
        
        var actualAmount = _sut.GetAmountOf(_beer);
        actualAmount.Should().Be(2);
        TestReport.Pass($"The total amount of {_beer} is currently {actualAmount} as expected");
        AttachScreenshot("IncreaseViaStepperButton", "Verify stepper increased the total amount");
        
        TestReport.LogInfo($"Decrease quantity of {_beer} via stepper button");
        _sut.DecreaseProductQuantity(_beer);

        actualAmount = _sut.GetAmountOf(_beer);
        actualAmount.Should().Be(1);
        TestReport.Pass($"The total amount of {_beer} is currently {actualAmount} as expected");
        AttachScreenshot("DecreaseViaStepperButton", "Verify stepper decreased the total amount");
    }

    [Test]
    public void RemoveProductFromOrder_AddingBeerTwiceToOrderRemoveItViaTrashcanSymbol_ShouldBeAnEmptyOrder()
    {
        TestReport.CreateNewTest("Remove from Order",$"Add {_beer} twice to order and remove it via button");
        _sut.OrderProductWith(name: _beer.Name);
        _sut.OrderProductWith(name: _beer.Name);
        var totalPrice = _sut.GetTotalPrice();
        var totalPriceAfterAddingBeerTwice = _beer.Price * 2;
        totalPrice.Should().Be(totalPriceAfterAddingBeerTwice);
        TestReport.Pass($"The total price of current order is {totalPrice} as expected ");
        
        TestReport.LogInfo($"Remove {_beer} from Order");
        _sut.RemoveProductFromOrder(_beer.Name);
        
        totalPrice = _sut.GetTotalPrice();
        totalPrice.Should().Be(0);
        TestReport.Pass("The total price is at 0, as expected");
    }

    [Test]
    public void
        RemoveProductFromOrder_AddingThreeProductsToOrderRemovingFirstProductViaStepper_ShouldOnlyRemoveFirstProduct()
    {
        TestReport.CreateNewTest("Remove From Order", 
            "Adding three products to the order and removing the fist one via stepper");
        TestReport.LogInfo($"Adding {_beer}, {_water} and {_soda} to current order");
        _sut.OrderProductWith(name: _beer.Name);
        _sut.OrderProductWith(name: _water.Name);
        _sut.OrderProductWith(name: _soda.Name);

        var actualPrice = _sut.GetTotalPrice();
        var expectedPrice = _beer.Price + _water.Price + _soda.Price;
        actualPrice.Should().Be(expectedPrice);
        TestReport.Pass($"The sub total price is {expectedPrice} as expected");
        
        TestReport.LogInfo($"Decrease quantity of {_beer} via stepper");
        _sut.DecreaseProductQuantity(_beer);
        
        var updatedPrice = _sut.GetTotalPrice();
        var expectedUpdatedPrice = _water.Price + _soda.Price;
        updatedPrice.Should().Be(expectedUpdatedPrice);
        TestReport.Pass($"The updated price is {updatedPrice} as expected");
    }
    
    protected override void TestSpecificTearDown()
    {
        var configurationView = BartenderCalculatorApp.OpenConfigurationView();
        configurationView.RemoveAllProducts();
    }
    
}