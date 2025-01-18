using Bartender.SqlLite.Extension;
using Bartender.SqlLite.Table;
using BartenderCalculator.Contracts;
using BartenderCalculator.Contracts.DTO;
using SQLite;

namespace Bartender.SqlLite;

public class Database: IDatabase
{
    private readonly SQLiteConnection _database;

    public Database(string dbPath)
    {
        _database = new SQLiteConnection(dbPath);
        _database.CreateTable<ProductTable>();
    }

    public void Insert(ProductDto product)
    {
        var productEntityToInsert = product.ToProduct();
        var productInDatabase = _database.Find<ProductTable>(productEntityToInsert.Id);
        if (productInDatabase == null)
        {
            _database.Insert(productEntityToInsert);
        }
    }

    public void Update(ProductDto product)
    {
        var productInDatabase = _database.Find<ProductTable>(product.Id);
        if(productInDatabase == null) return;
        productInDatabase.Name = product.Name;
        productInDatabase.Price = product.Price;
        _database.Update(productInDatabase);
    }

    public List<ProductDto> GetProducts()
    {
        var products = _database.Table<ProductTable>().ToList();
        return products.Select(product => product.ToProductDto()).ToList();
    }


    public void Delete(ProductDto product)
    {
        var productTableEntity = product.ToProduct();
        _database.Delete(productTableEntity);
    }

    public bool ContainsProduct(ProductDto product)
    {
        var productInDatabase = _database.Table<ProductTable>().Any(productEntity => productEntity.Name == product.Name);
        return productInDatabase;
    }
}