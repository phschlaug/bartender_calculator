using System.Collections.ObjectModel;
using System.Globalization;
using BartenderCalculator.Appium.Android.UiTests.AccessLayer.DTO;
using BartenderCalculator.Appium.Android.UiTests.AccessLayer.Exceptions;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

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

    public void DeleteProduct(string productName)
    {
        var productRow = GetRowIndexOfProductWithName(productName);
        if (productRow != -1)
        {
            DeleteProductAt(productRow);
        }
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

    public bool ContainsSavedProductWithName(string productName)
    {
        var createdProductTextFields = _driver.FindElements(By.Id(_configurationViewPath.ProductListViewNameTextFieldAccessId));
        foreach (var createdProductTextField in createdProductTextFields)
        {
            if (createdProductTextField.Text.Equals(productName))
            {
                return true;
            }
        }
        return false;
    }

    public void ChangeNameOfProductAt(int rowIndex, string toName)
    {
        var productRow = GetProductRowAtIndex(rowIndex);
        EditNameOfProduct(productRow, toName);
    }

    public void ChangeNameOfProductWithName(string currentName, string toName)
    {
        var indexOfProduct = GetRowIndexOfProductWithName(currentName);
        if (indexOfProduct is -1) throw new ProductNotExistingException(currentName);
        var productRow = GetProductRowAtIndex(indexOfProduct);
        EditNameOfProduct(productRow, toName);
    }

    public string GetProductNameAt(int rowIndex)
    {
        var rows = GetAllProductNameTextFieldsFromProdcutListView();
        if (rows is null) throw new ElementNotFoundException("ProductListViewRows");
        var productRow = rows[rowIndex];
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

    private ReadOnlyCollection<AppiumElement> GetAllProductNameTextFieldsFromProdcutListView()
    {
        return _driver.FindElements(By.Id(_configurationViewPath.ProductListViewNameTextFieldAccessId));
    }

    private int GetRowIndexOfProductWithName(string productName)
    {
        var products = _driver.FindElements(By.Id(_configurationViewPath.ProductListViewNameTextFieldAccessId));
        var productRow = -1;
        for (var rowIndex = 0; rowIndex < products.Count; rowIndex++)
        {
            var productTextField = products[rowIndex];
            if(productTextField.Text.Equals(productName))
            {
                productRow = rowIndex;
            } 
        }
        return productRow;
    }

    private AppiumElement GetProductRowAtIndex(int rowIndex)
    {
        var rows = GetAllProductNameTextFieldsFromProdcutListView();
        var productRow = rows[rowIndex];
        if (productRow is null) throw new ElementNotFoundException("ProductListViewRow");
        return productRow;
    }

    private void EditNameOfProduct(AppiumElement productRow, string newName)
    {
        var productNameTextFieldXPath =
            "//android.widget.EditText[@resource-id=\"com.phisch.bartendercalculator:id/ConfigurationView.ProductListView.NameTextField\"]";
        var productNameTextField = productRow.FindElement(By.XPath(productNameTextFieldXPath));
        productNameTextField.Clear();
        productNameTextField.SendKeys(newName);
        var editButton = productRow.FindElement(By.Id(_configurationViewPath.ProductListViewEditButtonAccessId));
        if(editButton is null) throw new ElementNotFoundException("ProductListViewRow.EditButton");
        editButton.Click();
    }
}