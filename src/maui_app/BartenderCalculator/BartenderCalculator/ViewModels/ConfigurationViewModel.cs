using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BartenderCalculator.Alert;
using BartenderCalculator.Contracts;
using BartenderCalculator.Contracts.DTO;
using BartenderCalculator.Resources.Strings;
using Microsoft.Extensions.Logging;

namespace BartenderCalculator.ViewModels;

public class ConfigurationViewModel: INotifyPropertyChanged
{
    public ObservableCollection<ProductDto> Products { get; set; }
    private string _productName;
    private decimal _productPrice;
    
    public ICommand AddProductCommand { get; set; }
    public ICommand DeleteProductCommand { get; set; }
    public ICommand EditProductCommand { get; set; }
    private readonly IDatabase _database;
    private readonly ILogger<ConfigurationViewModel> _logger;
    private readonly IMessenger _messenger;

    public ConfigurationViewModel(IDatabase database, ILogger<ConfigurationViewModel> logger, IMessenger messenger)
    {
        _database = database;
        _logger = logger;
        _messenger = messenger;
        _database.ProductsUpdated += OnProductUpdate;
        DeleteProductCommand = new Command<ProductDto>(DeleteProduct);
        AddProductCommand = new Command(AddProduct);
        EditProductCommand = new Command<ProductDto>(EditProduct);
        _productName = string.Empty;
        var savedProducts = _database.GetProducts();
        Products = new ObservableCollection<ProductDto>(savedProducts);
    }

    public string ProductName
    {
        get => _productName;
        set => SetProperty(ref _productName, value);
    }

    public decimal ProductPrice
    {
        get => _productPrice;
        set => SetProperty(ref _productPrice, value);
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void AddProduct()
    {
        if (ValidProductName(ProductName))
        {
            //Valid Product Name
            if(ValidProductPrice(ProductPrice))
            {
                //Valid input
                var productToAdd = new ProductDto { Name = ProductName, Price = ProductPrice };
                if (_database.ContainsProduct(productToAdd))
                {
                    // Error
                    _messenger.SendAsync(AppResources.Alert_WarningHeadline, 
                        AppResources.Alert_ProductSameNameExistsText, 
                        AppResources.Alert_OkButton);
                }
                else
                {
                    Products.Add(productToAdd);
                    _database.Insert(productToAdd);
                    ProductName = string.Empty;
                    ProductPrice = 0; 
                }
            }
            else
            {
                _logger.LogDebug("Valid product name, but price is invalid");
                _messenger.SendAsync(AppResources.Alert_InvalidProductPriceHeadline, 
                    AppResources.Alert_InvalidProductPrice_Text,
                    AppResources.Alert_OkButton);
            }
        }
        else
        {
            _logger.LogDebug("Product name is invalid");
            _messenger.SendAsync(AppResources.Alert_InvalidProductNameHeadline,
                AppResources.Alert_InvalidProductNameText,
                AppResources.Alert_OkButton);
        }
    }

    private void EditProduct(ProductDto product)
    {
        _database.Update(product);
    }

    private void DeleteProduct(ProductDto product)
    {
        if (!Products.Contains(product)) return;
        Products.Remove(product);
        _database.Delete(product);
    }

    private void SetProperty<T>(ref T backingField, T value, [CallerMemberName] string? propertyName = null)
    {
        if (Equals(backingField, value)) return;
        backingField = value;
        OnPropertyChanged(propertyName);
    }

    private void OnProductUpdate()
    {
        var products = _database.GetProducts();
        Products = new ObservableCollection<ProductDto>(products);
        OnPropertyChanged(nameof(Products));
    }
    
    private bool ValidProductName(string productName)
    {
        return !string.IsNullOrEmpty(productName);
    }

    private bool ValidProductPrice(decimal productPrice)
    {
        return productPrice != 0;
    }
}