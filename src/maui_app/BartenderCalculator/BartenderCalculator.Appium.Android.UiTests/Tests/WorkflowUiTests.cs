using BartenderCalculator.Appium.Android.UiTests.AccessLayer.BartenderApp.Views;
using BartenderCalculator.Appium.Android.UiTests.AccessLayer.DTO;
using FluentAssertions;

namespace BartenderCalculator.Appium.Android.UiTests.Tests;
/// <summary>
/// This class contains more 
/// </summary>
[TestFixture]
public class WorkflowUiTests: BaseUiTests
{
    private readonly ProductDto _beer = new("Beer", 8);
    private readonly ProductDto _water = new("Water", 2);
    private readonly ProductDto _wine = new("Wine", 9);

    [SetUp]
    public void Setup()
    {
        BartenderCalculatorApp.Start();
        var configurationView = BartenderCalculatorApp.ConfigurationView;
        configurationView.AddProduct(_beer);
        configurationView.AddProduct(_water);
        configurationView.AddProduct(_wine);
    }
    
    [Test]
    public void CreateOrderWithOneProductDeletingThisProduct_ShouldClearTheOrder()
    {
        var orderView = BartenderCalculatorApp.OpenOrderView();
        orderView.OrderProductWith("Beer");
        orderView.GetTotalPrice().Should().Be(_beer.Price);
        var configurationView = BartenderCalculatorApp.OpenConfigurationView();
        configurationView.IsVisible().Should().BeTrue();
        configurationView.DeleteProduct("Beer");
        configurationView.GetAmountOfProducts().Should().Be(2);
        orderView = BartenderCalculatorApp.OpenOrderView();
        orderView.GetTotalPrice().Should().Be(0);
    }

    [TearDown]
    public void TearDown()
    {
        var configView = BartenderCalculatorApp.OpenConfigurationView();
        configView.IsVisible().Should().BeTrue();
        configView.RemoveAllProducts();
    }
    
}