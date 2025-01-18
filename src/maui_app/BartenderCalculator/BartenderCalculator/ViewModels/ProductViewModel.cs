using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BartenderCalculator.ViewModels;

public class ProductViewModel: INotifyPropertyChanged
{
    private string _name;
    private decimal _price;
    private bool _isModified;
	public readonly int Id;
    
    public ProductViewModel(int id, string name, decimal price)
    {
        Id = id;
        _name = name;
        _price = price;
    }

    public ProductViewModel(string name, decimal price): this(0, name, price)
    {
    }
    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged();
                IsModified = true;
            }
        }
    }

    public decimal Price
    {
        get => _price;
        set
        {
            if (_price != value)
            {
                _price = value;
                OnPropertyChanged();
                IsModified = true;
            }
        }
    }

    public bool IsModified
    {
        get => _isModified;
        set
        {
            if (_isModified != value)
            {
                _isModified = value;
                OnPropertyChanged();
            }
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}