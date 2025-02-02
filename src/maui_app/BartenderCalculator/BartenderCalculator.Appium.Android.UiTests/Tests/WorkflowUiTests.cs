using BartenderCalculator.Appium.Android.UiTests.AccessLayer.DTO;
using FluentAssertions;

namespace BartenderCalculator.Appium.Android.UiTests.Tests;
/// <summary>
/// This class contains test which covers more elaborate Workflows of the App 
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
        TestReport.CreateNewTest("Create Order", 
            "Create an order and then delete product which is part of the order");
        var orderView = BartenderCalculatorApp.OpenOrderView();
        TestReport.LogInfo($"Adding {_beer} to the current order");
        orderView.OrderProductWith("Beer");
        orderView.GetTotalPrice().Should().Be(_beer.Price);
        TestReport.Pass("Beer was successfully added to current order");
        TestReport.LogInfo("Switch to configuration view");
        var configurationView = BartenderCalculatorApp.OpenConfigurationView();
        configurationView.IsVisible().Should().BeTrue();
        TestReport.Pass("Configuration View is visible");
        TestReport.LogInfo($"Remove {_beer} from the database");
        configurationView.DeleteProduct(_beer.Name);
        configurationView.GetAmountOfProducts().Should().Be(2);
        TestReport.Pass("Only two products are available");
        AttachScreenshot("OnlyTwoProductsStored", "Verify that only two products are stored");

        TestReport.LogInfo("Switching back to Order View");
        orderView = BartenderCalculatorApp.OpenOrderView();
        orderView.GetTotalPrice().Should().Be(0);
        TestReport.Pass($"Total price is 0 as expected");
        AttachScreenshot("OrderIsEmpty", "Verify that the order is empty");
    }

    protected override void TestSpecificTearDown()
    {
        var configView = BartenderCalculatorApp.OpenConfigurationView();
        configView.IsVisible().Should().BeTrue();
        configView.RemoveAllProducts();
    }
    
}