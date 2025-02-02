using BartenderCalculator.Appium.Android.UiTests.AccessLayer.BartenderApp.Views;
using BartenderCalculator.Appium.Android.UiTests.AccessLayer.DTO;
using FluentAssertions;
using FluentAssertions.Execution;

namespace BartenderCalculator.Appium.Android.UiTests.Tests;

public class ConfigurationViewUiTests: BaseUiTests
{
    private ConfigurationView _sut;
    private readonly ProductDto _dummyProduct = new("Dummy Product", 5.5);
    private readonly ProductDto _beer = new("Beer", 7.5);
    private readonly ProductDto _soda = new("Soda", 4);
    
    [SetUp]
    public void Setup()
    {
        BartenderCalculatorApp.Start();
        _sut = BartenderCalculatorApp.ConfigurationView;
    }

    [Test]
    [TestCase("Beer", 5.5)]
    [TestCase("Deposit", -5)]
    public void AddProduct_AddProductWithGivenNameAndPrice_ExpectToSeeTheProduct(string name, double  price)
    {
        var product = new ProductDto(name, price);
        TestReport.CreateNewTest("Add Product", $"Adding product with Name {name} and Price {price}");
        const int expectedAmountOfProducts = 1;
        TestReport.LogInfo($"Adding Product {product}");
        _sut.AddProduct(product);

        var actualAmountOfProducts = _sut.GetAmountOfProducts();

        actualAmountOfProducts.Should().Be(expectedAmountOfProducts);
        TestReport.Pass($"The product list contains the expected number of products of {actualAmountOfProducts}");
        AttachScreenshot("AddProductScreenshot", "Verify Product was added");
    }

    [Test]
    public void DeleteProduct_DeletingDummyProduct_ExpectToDeleteProduct()
    {
        TestReport.CreateNewTest("Delete Product", "Remove the added product");
        TestReport.LogInfo($"Add Product {_dummyProduct}");
        _sut.AddProduct(_dummyProduct);
        var amountOfProducts = _sut.GetAmountOfProducts();
        amountOfProducts.Should().Be(1);
        TestReport.Pass($"The amount of number is {amountOfProducts}, as expected");
        AttachScreenshot("ProveAddProductScreenshot", "Verify Product was added");
        
        TestReport.LogInfo($"Delete Product {_dummyProduct}");
        _sut.DeleteProductAt(0);
        
        _sut.GetAmountOfProducts().Should().Be(0);
        TestReport.Pass("The product list is empty, as expected");
        AttachScreenshot("ProveDeleteProductScreenshot", "Verify Product was removed");
    }

    [Test]
    public void EditProduct_AddDummyProductChangeNameToFooBar_ExpectProductToHaveTheNameFooBar()
    {
        TestReport.CreateNewTest("Edit Product", "Change the name of product to FoobBar");
        const string expectedName = "FooBar";
        TestReport.LogInfo($"Add {_dummyProduct}");
        _sut.AddProduct(_dummyProduct);
        TestReport.LogInfo($"Change name of {_dummyProduct.Name} to {expectedName}");
        _sut.ChangeNameOfProductWithName(_dummyProduct.Name, expectedName);
        
        var productName = _sut.GetProductNameAt(0);
        productName.Should().Be(expectedName);
        TestReport.Pass($"The product name is {productName}, as expected");
        AttachScreenshot("ChangedNameOfProduct", "Verify Product name was changed");
    }

    [Test]
    public void EditProduct_TryToRemoveNameOfProductAndSaveIt_ShouldDisplayAlertView()
    {
        TestReport.CreateNewTest("Edit Product", 
            "Remove the name of the product and try to save it, should not be possible to save");
        TestReport.LogInfo($"Add Product {_dummyProduct}");
        _sut.AddProduct(_dummyProduct);
        TestReport.LogInfo($"Remove the name of the product");
        _sut.ChangeNameOfProductWithName(_dummyProduct.Name, string.Empty);
        var alertView = BartenderCalculatorApp.GetAlertView();
        
        var isAlertViewVisible = alertView.IsVisible();
        isAlertViewVisible.Should().BeTrue();
        TestReport.Pass("An alert view is visible");
        AttachScreenshot("AlertViewEmptyName", "Verify Alert View indicates that the name was empty");
        alertView.CloseAlertView();
    }

    [Test]
    public void DeleteProduct_AddingTwoProductsThenRemovingBoth_ExpectEmptyProductList()
    {
        TestReport.CreateNewTest("Delete Product"," Adding two products, remove all products");
        TestReport.LogInfo($"Adding {_beer} and {_soda}");
        _sut.AddProduct(_beer);
        _sut.AddProduct(_soda);
        
        //Checking if two products are available
        TestReport.LogInfo("Checking if both products are added");
        var amountOfProducts = _sut.GetAmountOfProducts();
        amountOfProducts.Should().Be(2);
        
        TestReport.LogInfo("Delete the two added products");
        _sut.RemoveAllProducts();

        var products = _sut.GetAmountOfProducts();
        products.Should().Be(0);
        TestReport.Pass("The product list is empty");
        AttachScreenshot("ProductListIsEmpty", "Verify that product list is empty");
    }

    [Test]
    public void AddProduct_AddingProductWithSameName_ExpectToNotifyUserAccordingly()
    {
        TestReport.CreateNewTest("Add Product",
            "Try to add two products with same name, should only save one.");
        TestReport.LogInfo($"Adding {_dummyProduct}");
        _sut.AddProduct(_dummyProduct);
        TestReport.LogInfo($"Try to add {_dummyProduct} again.");
        _sut.AddProduct(_dummyProduct);
        var alertView = BartenderCalculatorApp.GetAlertView();
        if (alertView.IsVisible())
        {
            TestReport.LogInfo("An alert view is visible");
            AttachScreenshot("SameNameProductNameAlert", "Alert View visible indicates same product name");
            alertView.CloseAlertView();
        }
        else
        {
            _ = new FailReason("AlertView is not visible");
        }
        
        _sut.GetAmountOfProducts().Should().Be(1);
        TestReport.Pass("Only one product was added.");
        AttachScreenshot("CorrectNumberOfProducts", "Verify that only one product was added");
    }

    [Test]
    public void AddProduct_TryToAddProductWithEmptyNameAndPrice_ExpectToNotifyUserAccordingly()
    {
        TestReport.CreateNewTest("Add Product","Try to add product with invalid name and price");
        TestReport.LogInfo($"Adding product with empty name and price at 0");
        _sut.AddProduct(string.Empty, 0);
        var alertView = BartenderCalculatorApp.GetAlertView();
        if (alertView.IsVisible())
        {
            TestReport.LogInfo("An alert view is visible");
            AttachScreenshot("SameNameProductNameAlert", "Alert View visible indicates same product name");
            alertView.CloseAlertView();
        }
        else
        {
            TestReport.Fail("AlertView is not visible");
            _ = new FailReason("AlertView is not visible");
        }
        var actualAmountOfProducts = _sut.GetAmountOfProducts();
        actualAmountOfProducts.Should().Be(0);
        TestReport.Pass("The product list is empty");
        AttachScreenshot("ProductListIsEmpty", "Verify that product list is empty");
    }

    [Test]
    public void AddProduct_TryToAddProductWithEmptyNameButHavingPrice_ShouldNotAddProduct()
    {
        TestReport.CreateNewTest("Add Product","Try to add product with invalid name but valid price");
        TestReport.LogInfo($"Adding product with empty name and price at 5");
        _sut.AddProduct(string.Empty, 5);
        var alertView = BartenderCalculatorApp.GetAlertView();
        if (alertView.IsVisible())
        {
            TestReport.LogInfo("An alert view is visible");
            AttachScreenshot("InvalidNameNameValidPriceAlert", "Alert View visible indicates invalid product name");
            alertView.CloseAlertView();
        }
        else
        {
            _ = new FailReason("AlertView is not visible");
        }
        var actualAmountOfProducts = _sut.GetAmountOfProducts();
        actualAmountOfProducts.Should().Be(0);
        TestReport.Pass("The product list is empty");
        AttachScreenshot("EmptyProductList", "Verify that empty product list is empty");
    }

    protected override void TestSpecificTearDown()
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