using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderMgmt.Domain;

namespace OrderMgmt.DataAccess
{
    public class ProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Tüm ürünleri getir 
        public List<Product> GetAll()
        {
            var list = new List<Product>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using var cmd = new SqlCommand("SELECT ProductId, StockCode, StockName, IsActive, Price FROM Products", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    
                    int ordProductId = reader.GetOrdinal("ProductId");
                    int ordStockCode = reader.GetOrdinal("StockCode");
                    int ordStockName = reader.GetOrdinal("StockName");
                    int ordIsActive = reader.GetOrdinal("IsActive");
                    int ordPrice = reader.GetOrdinal("Price");

                    while (reader.Read())
                    {
                        var p = new Product
                        {
                            ProductId = reader.IsDBNull(ordProductId) ? 0 : reader.GetInt32(ordProductId),
                            StockCode = reader.IsDBNull(ordStockCode) ? string.Empty : reader.GetString(ordStockCode),
                            StockName = reader.IsDBNull(ordStockName) ? string.Empty : reader.GetString(ordStockName),
                            IsActive = reader.IsDBNull(ordIsActive) ? false : reader.GetBoolean(ordIsActive),
                            Price = reader.IsDBNull(ordPrice) ? 0m : reader.GetDecimal(ordPrice)
                        };

                        list.Add(p);
                    }
                }
            }
            return list;
        }

        // Tek ürün getir
        public Product GetById(int id)
        {
            Product product = null;
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using var cmd = new SqlCommand("SELECT ProductId, StockCode, StockName, IsActive, Price FROM Products WHERE ProductId=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        product = new Product
                        {
                            ProductId = reader["ProductId"] == DBNull.Value ? 0 : (int)reader["ProductId"],
                            StockCode = reader["StockCode"] == DBNull.Value ? "" : reader["StockCode"].ToString(),
                            StockName = reader["StockName"] == DBNull.Value ? "" : reader["StockName"].ToString(),
                            IsActive = reader["IsActive"] == DBNull.Value ? false : (bool)reader["IsActive"],
                            Price = reader["Price"] == DBNull.Value ? 0m : (decimal)reader["Price"]
                        };
                    }
                }
            }
            return product;
        }

        // Ekleme
        public void Add(Product product)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(
                    "INSERT INTO Products (StockCode, StockName, IsActive, Price) VALUES (@code, @name, @active, @price)", conn);
                cmd.Parameters.AddWithValue("@code", product.StockCode ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@name", product.StockName ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@active", product.IsActive);
                cmd.Parameters.AddWithValue("@price", product.Price);
                cmd.ExecuteNonQuery();
            }
        }

        // Güncelleme
        public void Update(Product product)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(
                    "UPDATE Products SET StockCode=@code, StockName=@name, IsActive=@active, Price=@price WHERE ProductId=@id", conn);
                cmd.Parameters.AddWithValue("@id", product.ProductId);
                cmd.Parameters.AddWithValue("@code", product.StockCode);
                cmd.Parameters.AddWithValue("@name", product.StockName);
                cmd.Parameters.AddWithValue("@active", product.IsActive);
                cmd.Parameters.AddWithValue("@price", product.Price);
                cmd.ExecuteNonQuery();
            }
        }

        // Silme
        public void Delete(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("DELETE FROM Products WHERE ProductId=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
