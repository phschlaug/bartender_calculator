using Bartender.SqlLite;
using BartenderCalculator.Contracts.DTO;
using FluentAssertions;

namespace BartenderCalculator.SqlLite.UnitTests;

[TestFixture]
public class DatabaseTests
{
    private Database _sut;

    private readonly ProductDto _beer = new("Beer", 8)
    {
        Id = 1
    };

    [SetUp]
    public void Setup()
    {
        _sut = new Database(":memory:");
    }
    
    [Test]
    public void Insert_StoringValidProduct_ShouldStoreProductToDatabase()
    {
        _sut.Insert(_beer);

        var products = _sut.GetProducts();
        
        products.Count.Should().Be(1);
        products[0].Name.Should().Be(_beer.Name);
        products[0].Price.Should().Be(_beer.Price);
    }

    [Test]
    public void Insert_TryToInsertSameProductTwice_ShouldOnlyAddItOnce()
    {
        _sut.Insert(_beer);
        _sut.Insert(_beer);
        
        var products = _sut.GetProducts();
        products.Count.Should().Be(1);
    }

    [Test]
    public void Update_UpdatingExistingProduct_ShouldSimplyUpdateExistingProduct()
    {
        var productDto = new ProductDto("Soda", 8)
        {
            Id = 1
        };
        _sut.Insert(productDto);
        var products = _sut.GetProducts();
        products.Count.Should().Be(1);
        products[0].Name.Should().Be("Soda");

        productDto.Name = "Water";
        
        _sut.Update(productDto);
        products = _sut.GetProducts();
        products.Count.Should().Be(1);
        products[0].Name.Should().Be("Water");
    }

    [Test]
    public void Update_TryToUpdateNoneExistingProduct_ShouldNotThrowException()
    {
        var updateAction = () => _sut.Update(_beer);
        
        updateAction.Should().NotThrow();
        var products = _sut.GetProducts();
        products.Count.Should().Be(0);
    }

    [Test]
    public void Delete_AddingAnDummyProductAndDeleteIt_ShouldDeleteProduct()
    {
        _sut.Insert(_beer);
        var products = _sut.GetProducts();
        products.Count.Should().Be(1);
        
        _sut.Delete(_beer);
        
        products = _sut.GetProducts();
        products.Count.Should().Be(0);
    }

    [Test]
    public void Delete_TryToDeleteAnNotExistingProduct_ShouldNotThrowException()
    {
        var deleteAction = () => _sut.Delete(_beer);

        deleteAction.Should().NotThrow();
    }

    [Test]
    public void ContainsProduct_CheckingEmptyDatabase_ShouldReturnFalse()
    {
        _sut.ContainsProduct(_beer).Should().BeFalse();
    }

    [Test]
    public void ContainsProduct_DatabaseContainsProduct_ShouldReturnTrue()
    {
        _sut.Insert(_beer);
        
        _sut.ContainsProduct(_beer).Should().BeTrue();
    }

    [Test]
    public void ContainsProduct_DatabaseContainsProductCheckingForProductSameNameDifferentId_ShouldReturnTrue()
    {
        var productDto = new ProductDto("Beer", 8)
        {
            Id = 1
        };
        var secondProductDto = new ProductDto("Beer", 8)
        {
            Id = 2
        };
        
        _sut.Insert(productDto);
        
        _sut.ContainsProduct(secondProductDto).Should().BeTrue();
    }
}