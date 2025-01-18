namespace BartenderCalculator.Contracts.DTO;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    public ProductDto(string name, decimal price)
    {
        Name = name;
        Price = price;
    }
}