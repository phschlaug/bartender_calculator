namespace BartenderCalculator.Appium.Android.UiTests.AccessLayer.DTO;

public class ProductDto(string name, double price)
{
    public string Name { get; set; } = name;
    public double Price { get; set; } = price;

    public override string ToString()
    {
        return $"Name: {Name}, Price: {Price}";
    }
}