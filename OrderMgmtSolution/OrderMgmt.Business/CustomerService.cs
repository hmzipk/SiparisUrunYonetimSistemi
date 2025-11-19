using OrderMgmt.DataAccess;
using OrderMgmt.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderMgmt.Business
{
    public class CustomerService
    {
        private readonly CustomerRepository _repo;

        public CustomerService(CustomerRepository repo)
        {
            _repo = repo;
        }


        // Tüm müşterileri getir
        public List<Customer> GetAllCustomers()
        {
            return _repo.GetAll();
        }

        // Tek müşteri getir
        public Customer GetCustomerById(int id)
        {
            return _repo.GetCustomerById(id);
        }

        // Yeni müşteri ekle
        public void AddCustomer(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.CustomerCode))
                throw new ArgumentException("CustomerCode boş olamaz!");
            if (string.IsNullOrWhiteSpace(customer.CustomerName))
                throw new ArgumentException("CustomerName boş olamaz!");
            if (string.IsNullOrWhiteSpace(customer.Email))
                throw new ArgumentException("Email boş olamaz!");

            customer.CreatedAt = DateTime.Now;
            _repo.Add(customer);
        }

        // Müşteri güncelle
        public void UpdateCustomer(Customer customer)
        {
            _repo.Update(customer);
        }

        // Müşteri sil
        public void DeleteCustomer(int id)
        {
            _repo.Delete(id);
        }
    }
}
