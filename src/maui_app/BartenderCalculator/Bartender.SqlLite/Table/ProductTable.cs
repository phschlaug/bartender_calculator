using SQLite;

namespace Bartender.SqlLite.Table;

public class ProductTable
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; } 
}