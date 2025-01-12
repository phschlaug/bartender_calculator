namespace BartenderCalculator.Contracts.DTO;

public class OrderItemDto
{
    public ProductDto Product { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice => Product.Price * Quantity;

    public OrderItemDto(ProductDto product, int quantity = 1)
    {
        Product = product;
        Quantity = quantity;
    }

    public override string ToString()
    {
        return $"Order Item Product: {Product.Name}, Quantity: {Quantity}, TotalPrice: {TotalPrice}";
    }
}