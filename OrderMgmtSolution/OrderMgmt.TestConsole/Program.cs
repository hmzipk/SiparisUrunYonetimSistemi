using System;
using OrderMgmt.DataAccess;

class Program
{
    static void Main(string[] args)
    {
        string connStr = "Server=HAMZA\\SQLEXPRESS;Database=OrderMgmtDb;User Id=hamza;Password=Hamza2025;TrustServerCertificate=True;";
        var repo = new ProductRepository(connStr);
        var list = repo.GetAll();
        Console.WriteLine($"Product count from repository: {list?.Count ?? 0}");
        foreach (var p in list)
        {
            Console.WriteLine($"Id={p.ProductId}, Name='{p.StockName}', Code='{p.StockCode}', Price={p.Price}, Active={p.IsActive}");
        }
    }
}