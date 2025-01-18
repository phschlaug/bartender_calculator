using System.ComponentModel;

namespace BartenderCalculator.ViewModels;

public class OrderItemViewModel: INotifyPropertyChanged
{
    private readonly ProductViewModel _product;
    private int _quantity;

    public OrderItemViewModel(ProductViewModel product)
    {
        _product = product;
        _quantity = 1;
    }
    
    public string Name => _product.Name;

    public int Quantity
    {
        get => _quantity;
        set
        {
            if (_quantity != value)
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
                OnPropertyChanged(nameof(TotalPrice));
            }
        }
    }
    
    public int Id => _product.Id;
    public decimal TotalPrice => _product.Price * _quantity;
    public decimal Price => _product.Price;
    public ProductViewModel Product => _product;
    
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}