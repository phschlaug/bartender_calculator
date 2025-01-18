using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BartenderCalculator.Alert;
using BartenderCalculator.Interface;
using BartenderCalculator.Resources.Strings;
using Microsoft.Extensions.Logging;

namespace BartenderCalculator.ViewModels;

public class ConfigurationViewModel: INotifyPropertyChanged
{
    public ObservableCollection<ProductViewModel> Products { get; set; }
    private string _productName;
    private decimal _productPrice;
    
    public ICommand AddProductCommand { get; set; }
    public ICommand DeleteProductCommand { get; set; }
    public ICommand EditProductCommand { get; set; }
    private readonly IProductService _productService;
    private readonly ILogger<ConfigurationViewModel> _logger;
    private readonly IMessenger _messenger;

    public ConfigurationViewModel(IProductService productService, ILogger<ConfigurationViewModel> logger, IMessenger messenger)
    {
        _productService = productService;
        _logger = logger;
        
        _messenger = messenger;
        DeleteProductCommand = new Command<ProductViewModel>(DeleteProduct);
        AddProductCommand = new Command(AddProduct);
        EditProductCommand = new Command<ProductViewModel>(EditProduct);
        _productName = string.Empty;
        var savedProducts = _productService.GetAllProducts(); 
        Products = new ObservableCollection<ProductViewModel>(savedProducts);
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
                var productToAdd = new ProductViewModel(ProductName, ProductPrice);
                if(Products.Any(p => p.Name == productToAdd.Name))
                {
                    _messenger.SendAsync(AppResources.Alert_WarningHeadline, 
                        AppResources.Alert_ProductSameNameExistsText, 
                        AppResources.Alert_OkButton);
                }
                else
                {
                    Products.Add(productToAdd);
                    _productService.SaveProduct(productToAdd);
                    ProductName = string.Empty;
                    ProductPrice = 0;
                    RefreshProductList();
                }
            }
            else
            {
                DisplayInvalidPriceWithValidNameAlert();
            }
        }
        else
        {
            DisplayInvalidProductNameAlert();
        }
    }

    private void DisplayInvalidPriceWithValidNameAlert()
    {
        _logger.LogDebug("Valid product name, but price is invalid");
        _messenger.SendAsync(AppResources.Alert_InvalidProductPriceHeadline, 
            AppResources.Alert_InvalidProductPrice_Text,
            AppResources.Alert_OkButton);
    }

    private void DisplayInvalidProductNameAlert()
    {
        _logger.LogDebug("Product name is invalid");
        _messenger.SendAsync(AppResources.Alert_InvalidProductNameHeadline,
            AppResources.Alert_InvalidProductNameText,
            AppResources.Alert_OkButton);
    }

    private void EditProduct(ProductViewModel product)
    {
        if (!ValidProductName(product.Name))
        {
            DisplayInvalidProductNameAlert();
            RefreshProductList();
            return;
        }

        if (!ValidProductPrice(product.Price))
        {
            DisplayInvalidPriceWithValidNameAlert();
            RefreshProductList();
            return;
        }
        _productService.UpdateProduct(product);
        product.IsModified = false;
        RefreshProductList();
    }

    private void DeleteProduct(ProductViewModel product)
    {
        if (!Products.Contains(product)) return;
        Products.Remove(product);
        _productService.DeleteProduct(product);
        RefreshProductList();
    }

    private void SetProperty<T>(ref T backingField, T value, [CallerMemberName] string? propertyName = null)
    {
        if (Equals(backingField, value)) return;
        backingField = value;
        OnPropertyChanged(propertyName);
    }
    
    private bool ValidProductName(string productName)
    {
        return !string.IsNullOrEmpty(productName);
    }

    private bool ValidProductPrice(decimal productPrice)
    {
        return productPrice != 0;
    }

    private void RefreshProductList()
    {
        Products.Clear();
        foreach (var product in _productService.GetAllProducts())
        {
            Products.Add(product);
        }
    }
}