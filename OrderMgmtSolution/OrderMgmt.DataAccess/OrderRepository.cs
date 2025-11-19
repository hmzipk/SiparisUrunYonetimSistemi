using Microsoft.Data.SqlClient;
using OrderMgmt.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderMgmt.DataAccess
{
    public class OrderRepository
    {
        private readonly string _connectionString;
        public OrderRepository(string connectionString) => _connectionString = connectionString;

        //Tüm siparişleri getir
        public List<Order> GetAll()
        {
            var list = new List<Order>();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            var cmd = new SqlCommand("SELECT * FROM Orders ORDER BY OrderId DESC", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Order
                {
                    OrderId = (int)reader["OrderId"],
                    OrderNo = reader["OrderNo"].ToString()!,
                    CustomerId = (int)reader["CustomerId"],
                    ProductId = (int)reader["ProductId"],
                    Quantity = (int)reader["Quantity"],
                    UnitPrice = (decimal)reader["UnitPrice"],
                    TotalAmount = (decimal)reader["TotalAmount"],
                    CreatedAt = (DateTime)reader["CreatedAt"],
                    DeliveryConfirmed = (bool)reader["DeliveryConfirmed"],
                    DeliveryConfirmToken = reader["DeliveryConfirmToken"]?.ToString()
                });
            }
            return list;
        }

        //Yeni sipariş ekle
        public void Add(Order order)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            var cmd = new SqlCommand(
                "INSERT INTO Orders (OrderNo, CustomerId, ProductId, Quantity, UnitPrice, TotalAmount, CreatedAt, DeliveryConfirmed, DeliveryConfirmToken) " +
                "VALUES (@no, @cid, @pid, @qty, @unit, @total, @created, @confirmed, @token)", conn);

            cmd.Parameters.AddWithValue("@no", order.OrderNo);
            cmd.Parameters.AddWithValue("@cid", order.CustomerId);
            cmd.Parameters.AddWithValue("@pid", order.ProductId);
            cmd.Parameters.AddWithValue("@qty", order.Quantity);
            cmd.Parameters.AddWithValue("@unit", order.UnitPrice);
            cmd.Parameters.AddWithValue("@total", order.TotalAmount);
            cmd.Parameters.AddWithValue("@created", order.CreatedAt);
            cmd.Parameters.AddWithValue("@confirmed", order.DeliveryConfirmed);
            cmd.Parameters.AddWithValue("@token", (object?)order.DeliveryConfirmToken ?? DBNull.Value);

            cmd.ExecuteNonQuery();
        }

        //Token ile sipariş getir
        public Order? GetByToken(string token)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            var cmd = new SqlCommand("SELECT * FROM Orders WHERE DeliveryConfirmToken=@t", conn);
            cmd.Parameters.AddWithValue("@t", token);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Order
                {
                    OrderId = (int)reader["OrderId"],
                    OrderNo = reader["OrderNo"].ToString()!,
                    CustomerId = (int)reader["CustomerId"],
                    ProductId = (int)reader["ProductId"],
                    Quantity = (int)reader["Quantity"],
                    UnitPrice = (decimal)reader["UnitPrice"],
                    TotalAmount = (decimal)reader["TotalAmount"],
                    CreatedAt = (DateTime)reader["CreatedAt"],
                    DeliveryConfirmed = (bool)reader["DeliveryConfirmed"],
                    DeliveryConfirmToken = reader["DeliveryConfirmToken"]?.ToString()
                };
            }
            return null;
        }

        //Sipariş teslimat onayını güncelle
        public void UpdateDeliveryConfirmed(int orderId, bool confirmed)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            var cmd = new SqlCommand("UPDATE Orders SET DeliveryConfirmed=@c WHERE OrderId=@id", conn);
            cmd.Parameters.AddWithValue("@id", orderId);
            cmd.Parameters.AddWithValue("@c", confirmed);
            cmd.ExecuteNonQuery();
        }

        //Siparişi sil
        public void Delete(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            var cmd = new SqlCommand("DELETE FROM Orders WHERE OrderId=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        public Order GetById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT OrderId, CustomerId, ProductId, Quantity, UnitPrice, TotalAmount, OrderNo, CreatedAt, DeliveryConfirmed FROM Orders WHERE OrderId=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Order
                        {
                            OrderId = (int)reader["OrderId"],
                            CustomerId = (int)reader["CustomerId"],
                            ProductId = (int)reader["ProductId"],
                            Quantity = (int)reader["Quantity"],
                            UnitPrice = (decimal)reader["UnitPrice"],
                            TotalAmount = (decimal)reader["TotalAmount"],
                            OrderNo = reader["OrderNo"].ToString(),
                            CreatedAt = (DateTime)reader["CreatedAt"],
                            DeliveryConfirmed = (bool)reader["DeliveryConfirmed"]
                        };
                    }
                }
            }
            return null;
        }

    }
}

