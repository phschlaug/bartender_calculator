using BartenderCalculator.Appium.Android.UiTests.AccessLayer.BartenderApp.Views;
using FluentAssertions;
using FluentAssertions.Execution;

namespace BartenderCalculator.Appium.Android.UiTests.Tests;

public class ConfigurationViewUiTests: BaseUiTests
{
    private ConfigurationView _sut;
    
    [SetUp]
    public void Setup()
    {
        BartenderCalculatorApp.Start();
        _sut = BartenderCalculatorApp.ConfigurationView;
    }

    [Test]
    public void AddProduct_AddingOneDummyProduct_ExpectToSeeProduct()
    {
        const int expectedAmountOfProducts = 1;
        
        _sut.AddProduct("Dummy Product", 2.5);

        var actualAmountOfProducts = _sut.GetAmountOfProducts();
        
        actualAmountOfProducts.Should().Be(expectedAmountOfProducts);
    }

    [Test]
    public void DeleteProduct_DeletingDummyProduct_ExpectToDeleteProduct()
    {
        _sut.AddProduct("Dummy Product", 5.5);
        var amountOfProducts = _sut.GetAmountOfProducts();
        amountOfProducts.Should().Be(1);
        
        _sut.DeleteProductAt(0);
        
        _sut.GetAmountOfProducts().Should().Be(0);
    }

    [Test]
    public void EditProduct_AddDummyProductChangeNameToFooBar_ExpectProductToHaveTheNameFooBar()
    {
        const string expectedName = "FooBar";
        _sut.AddProduct("Dummy Product", 2.5);
        _sut.ChangeNameOfProductAt(0, expectedName);
        var productName = _sut.GetProductNameAt(0);
       
        productName.Should().Be(expectedName);
    }

    [Test]
    public void DeleteProduct_AddingTwoProductsThenRemovingBoth_ExpectEmptyProductList()
    {
        _sut.AddProduct("Foo", 2.5);
        _sut.AddProduct("Bar", 2.5);
        
        //Checking if two products are available
        var amountOfProducts = _sut.GetAmountOfProducts();
        amountOfProducts.Should().Be(2);
        
        _sut.RemoveAllProducts();

        var products = _sut.GetAmountOfProducts();

        products.Should().Be(0);
    }

    [Test]
    public void AddProduct_AddingProductWithSameName_ExpectToNotifyUserAccordingly()
    {
        _sut.AddProduct("Foo", 2.5);
        _sut.AddProduct("Foo", 2.5);
        var alertView = BartenderCalculatorApp.GetAlertView();
        if (alertView.IsVisible())
        {
            alertView.CloseAlertView();
        }
        else
        {
            var _ = new FailReason("AlertView is not visible");
        }
        _sut.GetAmountOfProducts().Should().Be(1);
    }

    [Test]
    public void AddProduct_TryToAddProductWithEmptyNameAndPrice_ExpectToNotifyUserAccordingly()
    {
        _sut.AddProduct(string.Empty, 0);
        var alertView = BartenderCalculatorApp.GetAlertView();
        alertView.IsVisible().Should().BeTrue();
        alertView.CloseAlertView();
        alertView.IsVisible().Should().BeFalse();
    }

    [Test]
    public void AddProduct_TryToAddProductWithEmptyNameButHavingPrice_ShouldNotAddProduct()
    {
        _sut.AddProduct(string.Empty, 5);
        var actualAmountOfProducts = _sut.GetAmountOfProducts();
        var alertView = BartenderCalculatorApp.GetAlertView();
        alertView.IsVisible().Should().BeTrue();
        alertView.CloseAlertView();
        actualAmountOfProducts.Should().Be(0);
    }

    [TearDown]
    public void TearDown()
    {
        CleanUpProductListIfNeeded();
        BartenderCalculatorApp.Stop();
    }

    private void CleanUpProductListIfNeeded()
    {
        if (_sut.GetAmountOfProducts() > 0)
        {
            _sut.RemoveAllProducts();
        }
    }
}