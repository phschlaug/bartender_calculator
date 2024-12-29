using BartenderCalculator.Appium.Android.UiTests.AccessLayer.DTO;
using BartenderCalculator.Appium.Android.UiTests.AccessLayer.Helper;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;

namespace BartenderCalculator.Appium.Android.UiTests.AccessLayer.BartenderApp.Views;

internal struct OrderViewPath
{
    public readonly string TotalPriceLabel = "OrderView.TotalPriceLabel";
    public string AvailableProductList = "OrderView.AvailableProducts";
    public readonly string AvailableProductsAddToOrderButton = "OrderView.AvailableProducts.AddToOrderButton";
    public readonly string ClearOrderButton = "OrderView.ClearOrderButton";
    // Current Order Section
    public readonly string RemoveProductFromOrderButton = "OrderView.CurrentOrder.RemoveFromOrderButton";
    public readonly string CurrentOrderProductStepper = "OrderView.CurrentOrder.Stepper";
    public readonly string CurrentOrderProductNameLabel = "OrderView.CurrentOrder.ProductNameLabel";
    public string CurrentOderAmountStepper = "OrderView.CurrentOrder.Stepper";
    public readonly string CurrentOderStepperDecreaseButtonXPath = "//android.widget.Button[@content-desc=\"\u2212\"]";
    public readonly string CurrentOrderStepperIncreaseButtonXPath = "\t\n//android.widget.Button[@content-desc=\"+\"]";
    public readonly string CurrentOrderAmountLabel = "OrderView.CurrentOrder.AmountLabel";

    public OrderViewPath()
    {
    }
}

public class OrderView: IView
{
    private readonly OrderViewPath _path;
    private readonly AndroidDriver _driver;

    public OrderView(AndroidDriver driver)
    {
        _driver = driver;
        _path = new OrderViewPath();
    }

    public double GetTotalPrice()
    {
        var totalPriceTextField = _driver.FindElement(By.Id(_path.TotalPriceLabel));
        var totalPriceText = totalPriceTextField.Text;
        return StringParser.ParsePrice(totalPriceText);
    }
    
    public void OrderProductWith(string name)
    {
        var productButtons = _driver.FindElements(By.Id(_path.AvailableProductsAddToOrderButton));
        foreach (var productButton in productButtons)
        {
            if (productButton.Text == name)
            {
                productButton.Click();
            }
        }
    }

    public bool IsVisible()
    {
       var totalPriceLabel = _driver.FindElement(By.Id(_path.TotalPriceLabel));
       var totalPriceLabelIsDisplayed = totalPriceLabel.Displayed;
       return totalPriceLabelIsDisplayed;
    }

    public void RemoveProductFromOrder(string productName)
    {
        var index = GetProductIndexByName(productName);
        var removeProductFromOrderButtons = _driver.FindElements(By.Id(_path.RemoveProductFromOrderButton));
        removeProductFromOrderButtons[index].Click();
    }

    public void ClearOrder()
    {
        var clearOrderButton = _driver.FindElement(By.Id(_path.ClearOrderButton));
        clearOrderButton?.Click();
    }

    public void IncreaseProductQuantity(string productName)
    {
        var productStepper = GetStepperFor(productName);
        var increaseButton = productStepper.FindElement(By.XPath(_path.CurrentOrderStepperIncreaseButtonXPath));
        increaseButton.Click();
    }

    public void DecreaseProductQuantity(ProductDto product)
    {
        var productStepper = GetStepperFor(product.Name);
        var decreaseButton = productStepper.FindElement(By.XPath(_path.CurrentOderStepperDecreaseButtonXPath));
        decreaseButton.Click();
    }

    public int GetAmountOf(ProductDto product)
    {
        var productIndex = GetProductIndexByName(product.Name);
        var productAmountLabels = _driver.FindElements(By.Id(_path.CurrentOrderAmountLabel));
        var productAmountLabel = productAmountLabels[productIndex];
        var text = productAmountLabel.Text;
        return int.Parse(text);
    }

    private int GetProductIndexByName(string productName)
    {
        var productNameLabels = _driver.FindElements(By.Id(_path.CurrentOrderProductNameLabel));
        for (var i = 0; i <= productNameLabels.Count; i++)
        {
            var productNameLabel = productNameLabels[i];
            if (productNameLabel.Text == productName)
            {
                return i;
            }
        }
        // ToDo: Add dedicated Exception here
        throw new Exception("Product not found");
    }

    private AppiumElement GetStepperFor(string productName)
    {
        var productIndex = GetProductIndexByName(productName);
        var steppers = _driver.FindElements(By.Id(_path.CurrentOrderProductStepper));
        var productStepper = steppers[productIndex];
        return productStepper;
    }
}