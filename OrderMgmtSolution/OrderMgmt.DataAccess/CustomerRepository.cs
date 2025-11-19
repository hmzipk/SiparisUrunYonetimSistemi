using Microsoft.Data.SqlClient;
using OrderMgmt.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderMgmt.DataAccess
{
    public class CustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(string connStr)
        {
            _connectionString = connStr;
        }


        // Tüm müşterileri listele
        public List<Customer> GetAll()
        {
            var customers = new List<Customer>();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand("SELECT CustomerId, CustomerCode, CustomerName, Email, CreatedAt, IsActive FROM Customers", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                customers.Add(new Customer
                {
                    CustomerId = (int)reader["CustomerId"],
                    CustomerCode = reader["CustomerCode"].ToString(),
                    CustomerName = reader["CustomerName"].ToString(),
                    Email = reader["Email"].ToString(),
                    CreatedAt = (DateTime)reader["CreatedAt"],
                    IsActive = (bool)reader["IsActive"]
                });
            }
            return customers;
        }

        // Id ile müşteri bul
        public Customer GetCustomerById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand("SELECT CustomerId, CustomerCode, CustomerName, Email, CreatedAt, IsActive FROM Customers WHERE CustomerId=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Customer
                {
                    CustomerId = (int)reader["CustomerId"],
                    CustomerCode = reader["CustomerCode"].ToString(),
                    CustomerName = reader["CustomerName"].ToString(),
                    Email = reader["Email"].ToString(),
                    CreatedAt = (DateTime)reader["CreatedAt"],
                    IsActive = (bool)reader["IsActive"]
                };
            }
            return null;
        }

        // Yeni müşteri ekle
        public void Add(Customer customer)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand(
                "INSERT INTO Customers (CustomerCode, CustomerName, Email, CreatedAt, IsActive) VALUES (@code, @name, @mail, @created, @active)", conn);
            cmd.Parameters.AddWithValue("@code", customer.CustomerCode);
            cmd.Parameters.AddWithValue("@name", customer.CustomerName);
            cmd.Parameters.AddWithValue("@mail", customer.Email ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@created", customer.CreatedAt == default ? DateTime.Now : customer.CreatedAt);
            cmd.Parameters.AddWithValue("@active", customer.IsActive);
            cmd.ExecuteNonQuery();
        }

        // Müşteri güncelle
        public void Update(Customer customer)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand(
                "UPDATE Customers SET CustomerCode=@code, CustomerName=@name, Email=@mail, IsActive=@active WHERE CustomerId=@id", conn);
            cmd.Parameters.AddWithValue("@id", customer.CustomerId);
            cmd.Parameters.AddWithValue("@code", customer.CustomerCode);
            cmd.Parameters.AddWithValue("@name", customer.CustomerName);
            cmd.Parameters.AddWithValue("@mail", customer.Email ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@active", customer.IsActive);
            cmd.ExecuteNonQuery();
        }

        // Müşteri sil
        public void Delete(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand("DELETE FROM Customers WHERE CustomerId=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        // Email ile müşteri bul
        public Customer GetByEmail(string email)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand("SELECT CustomerId, CustomerCode, CustomerName, Email, CreatedAt, IsActive FROM Customers WHERE Email=@mail", conn);
            cmd.Parameters.AddWithValue("@mail", email);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Customer
                {
                    CustomerId = (int)reader["CustomerId"],
                    CustomerCode = reader["CustomerCode"].ToString(),
                    CustomerName = reader["CustomerName"].ToString(),
                    Email = reader["Email"].ToString(),
                    CreatedAt = (DateTime)reader["CreatedAt"],
                    IsActive = (bool)reader["IsActive"]
                };
            }
            return null;
        }

    }
}



