using System.Collections.ObjectModel;
using System.Globalization;
using BartenderCalculator.Appium.Android.UiTests.AccessLayer.DTO;
using BartenderCalculator.Appium.Android.UiTests.AccessLayer.Exceptions;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.DevTools.V130.Page;

namespace BartenderCalculator.Appium.Android.UiTests.AccessLayer.BartenderApp.Views;

internal struct ConfigurationViewPath
{
    public string ProductNameTextFieldAccessId => "ConfigurationView.ProductName";
    public string ProductPriceTextFieldAccessId => "ConfigurationView.ProductPriceTextField"; 
    public string AddProductButtonAccessId => "ConfigurationView.AddProductButton";
    public string ProductListViewAccessId => "ConfigurationView.ProductListView";
    
    // ProductListView
    public string ProductListViewNameTextFieldAccessId => "ConfigurationView.ProductListView.NameTextField";
    public string ProductListViewPriceTextFieldAccessId => "ConfigurationView.ProductListView.PriceTextField";
    public string ProductListViewEditButtonAccessId => "ConfigurationView.ProductListView.EditProductButton"; 
    public string ProductListViewDeleteButtonAccessId => "ConfigurationView.ProductListView.DeleteButton";
}
public class ConfigurationView: IView
{
    private readonly AndroidDriver _driver;
    private readonly ConfigurationViewPath _configurationViewPath;
    private readonly string rowsIndicator = "//android.widget.ListView/android.view.ViewGroup";

    public ConfigurationView(AndroidDriver driver)
    {
        _driver = driver;
        _configurationViewPath = new ConfigurationViewPath();
    }

    private void EnterProductName(string name)
    { 
        var productNameTextField = _driver.FindElement(By.Id(_configurationViewPath.ProductNameTextFieldAccessId));
        productNameTextField.SendKeys(name);
    }

    private void EnterProductPrice(double price)
    {
        var productPriceTextField = _driver.FindElement(By.Id(_configurationViewPath.ProductPriceTextFieldAccessId));
        productPriceTextField.SendKeys(price.ToString(CultureInfo.InvariantCulture));
    }

    private void ClickAddProductButton()
    {
        var addProductButton = _driver.FindElement(By.Id(_configurationViewPath.AddProductButtonAccessId));
        addProductButton.Click();
    }

    public void AddProduct(string name, double productPrice)
    {
        EnterProductName(name);
        EnterProductPrice(productPrice);
        ClickAddProductButton();
    }
    public void AddProduct(ProductDto product)
    {
        AddProduct(product.Name, product.Price);
    }

    public void DeleteProductAt(int rowIndex)
    {
        var deleteButtonsOfProductList = GetDeleteButtonsOfProductList(); 
        if(deleteButtonsOfProductList is null) throw new ElementNotFoundException("DeleteButtonOfProductList");
        var deleteButton = deleteButtonsOfProductList[rowIndex];
        deleteButton.Click();
    }

    public void RemoveAllProducts()
    {
        
        var deleteButtonsOfProductList = GetDeleteButtonsOfProductList();
        while (deleteButtonsOfProductList.Count > 0)
        {
            var deleteButton = deleteButtonsOfProductList[0];
            deleteButton.Click();
            deleteButtonsOfProductList = GetDeleteButtonsOfProductList();
        }
    }

    public int GetAmountOfProducts()
    {
        var rows = GetDeleteButtonsOfProductList();
        if (rows is null) throw new ElementNotFoundException("DeleteButtonOfProductList");
        return rows.Count;
    }

    public void ChangeNameOfProductAt(int rowIndex, string toName)
    {
        var rows = GetProductNameTextFieldFromProductsListView();
        var productRow = rows?[rowIndex];
        if (productRow is null) throw new ElementNotFoundException("ProductListViewRow");
        var productNameTextField = productRow.FindElement(By.Id(_configurationViewPath.ProductListViewNameTextFieldAccessId));
        productNameTextField.Clear();
        productNameTextField.SendKeys(toName);
        var editButton = productRow.FindElement(By.Id(_configurationViewPath.ProductListViewEditButtonAccessId));
        if(editButton is null) throw new ElementNotFoundException("ProductListViewRow.EditButton");
        editButton.Click();
    }

    public string GetProductNameAt(int rowIndex)
    {
        var rows = GetProductNameTextFieldFromProductsListView();
        if (rows is null) throw new ElementNotFoundException("ProductListViewRows");
        var productRow = rows?[rowIndex];
        if (productRow is null) throw new ElementNotFoundException("ProductListViewRow");
        var productName = productRow.FindElement(By.Id(_configurationViewPath.ProductListViewNameTextFieldAccessId));
        return productName.Text;
    }

    public bool IsVisible()
    {
       var addProductButton = _driver.FindElement(By.Id(_configurationViewPath.AddProductButtonAccessId));
       var buttonIsDisplayed = addProductButton.Displayed;
       return buttonIsDisplayed;
    }

    private ReadOnlyCollection<AppiumElement> GetDeleteButtonsOfProductList()
    {
        return _driver.FindElements(By.Id(_configurationViewPath.ProductListViewDeleteButtonAccessId));
    }

    private ReadOnlyCollection<AppiumElement> GetProductNameTextFieldFromProductsListView()
    {
        return _driver.FindElements(By.Id(_configurationViewPath.ProductListViewNameTextFieldAccessId));
    }
}