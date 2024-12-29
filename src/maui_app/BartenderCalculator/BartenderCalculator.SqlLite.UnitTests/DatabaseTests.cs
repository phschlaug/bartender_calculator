using Bartender.SqlLite;
using BartenderCalculator.Contracts.DTO;
using FluentAssertions;

namespace BartenderCalculator.SqlLite.UnitTests;

[TestFixture]
public class DatabaseTests
{
    private Database _sut;

    [SetUp]
    public void Setup()
    {
        _sut = new Database(":memory:");
    }
    
    [Test]
    public void Insert_StoringValidProduct_ShouldStoreProductToDatabase()
    {
        var productDto = new ProductDto{ Id = 1, Name="Beer", Price=8};
        
        _sut.Insert(productDto);

        var products = _sut.GetProducts();
        
        products.Count.Should().Be(1);
        products[0].Name.Should().Be(productDto.Name);
        products[0].Price.Should().Be(productDto.Price);
    }

    [Test]
    public void Insert_TryToInsertSameProductTwice_ShouldOnlyAddItOnce()
    {
        var productDto = new ProductDto{ Id = 1, Name="Beer", Price=8};
        _sut.Insert(productDto);
        _sut.Insert(productDto);
        
        var products = _sut.GetProducts();
        products.Count.Should().Be(1);
    }

    [Test]
    public void Update_UpdatingExistingProduct_ShouldSimplyUpdateExistingProduct()
    {
        var productDto = new ProductDto{ Id = 1, Name="Beer", Price=8};
        _sut.Insert(productDto);
        var products = _sut.GetProducts();
        products.Count.Should().Be(1);
        products[0].Name.Should().Be("Beer");

        productDto.Name = "Water";
        
        _sut.Update(productDto);
        products = _sut.GetProducts();
        products.Count.Should().Be(1);
        products[0].Name.Should().Be("Water");
    }

    [Test]
    public void Update_TryToUpdateNoneExistingProduct_ShouldNotThrowException()
    {
        var productDto = new ProductDto{ Id = 1, Name="Beer", Price=8};
        
        var updateAction = () => _sut.Update(productDto);
        
        updateAction.Should().NotThrow();
        var products = _sut.GetProducts();
        products.Count.Should().Be(0);
    }

    [Test]
    public void Delete_AddingAnDummyProductAndDeleteIt_ShouldDeleteProduct()
    {
        var productDto = new ProductDto{ Id = 1, Name="Beer", Price=8};
        _sut.Insert(productDto);
        var products = _sut.GetProducts();
        products.Count.Should().Be(1);
        
        _sut.Delete(productDto);
        
        products = _sut.GetProducts();
        products.Count.Should().Be(0);
    }

    [Test]
    public void Delete_TryToDeleteAnNotExistingProduct_ShouldNotThrowException()
    {
        var productDto = new ProductDto{ Id = 1, Name="Beer", Price=8};
        
        var deleteAction = () => _sut.Delete(productDto);

        deleteAction.Should().NotThrow();
    }

    [Test]
    public void ContainsProduct_CheckingEmptyDatabase_ShouldReturnFalse()
    {
        var notExistingProductDto = new ProductDto{ Id = 1, Name="Beer", Price=8};
        _sut.ContainsProduct(notExistingProductDto).Should().BeFalse();
    }

    [Test]
    public void ContainsProduct_DatabaseContainsProduct_ShouldReturnTrue()
    {
        var productDto = new ProductDto{ Id = 1, Name="Beer", Price=8};
        _sut.Insert(productDto);
        
        _sut.ContainsProduct(productDto).Should().BeTrue();
    }

    [Test]
    public void ContainsProduct_DatabaseContainsProductCheckingForProductSameNameDifferentId_ShouldReturnTrue()
    {
        var productDto = new ProductDto{ Id = 1, Name="Beer", Price=8};
        var secondProductDto = new ProductDto{ Id = 2, Name="Beer", Price=8};
        
        _sut.Insert(productDto);
        
        _sut.ContainsProduct(secondProductDto).Should().BeTrue();
    }
}