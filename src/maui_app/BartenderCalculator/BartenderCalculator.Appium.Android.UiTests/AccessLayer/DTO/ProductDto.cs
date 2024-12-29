namespace BartenderCalculator.Appium.Android.UiTests.AccessLayer.DTO;

public class ProductDto
{
    public string Name { get; set; }
    public double Price { get; set; }

    public ProductDto(string name, double price)
    {
        Name = name;
        Price = price;
    }
}