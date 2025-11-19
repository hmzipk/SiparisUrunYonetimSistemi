using OrderMgmt.DataAccess;
using OrderMgmt.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderMgmt.Business
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepo;
        private readonly ProductRepository _productRepo;
        private readonly CustomerRepository _customerRepo;
        private readonly EmailService _emailService;
        
        public OrderService(OrderRepository orderRepo, ProductRepository productRepo,
                            CustomerRepository customerRepo, EmailService emailService)
        {
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _customerRepo = customerRepo;
            _emailService = emailService;
        }

        public void AddOrder(Order order)
        {
            var customer = _customerRepo.GetCustomerById(order.CustomerId)
                ?? throw new Exception("Müşteri bulunamadı.");
            var product = _productRepo.GetById(order.ProductId)
                ?? throw new Exception("Ürün bulunamadı.");

            order.Customer = customer;
            order.Product = product;

            // Fiyatı Products.Price'tan al
            order.UnitPrice = product.Price;
            order.TotalAmount = order.UnitPrice * order.Quantity;

            // Sipariş bilgileri
            order.OrderNo = $"ORD{DateTime.Now:yyyyMMddHHmmss}";
            order.CreatedAt = DateTime.Now;
            order.DeliveryConfirmToken = Guid.NewGuid().ToString();
            order.DeliveryConfirmed = false;

            // Kaydet
            _orderRepo.Add(order);

            // E-posta gönder
            _emailService.SendOrderConfirmation(customer, order);
        }

        public Order ConfirmDelivery(string token)
        {
            var order = _orderRepo.GetByToken(token);
            if (order == null)
                throw new Exception("Invalid token");

            order.Product = _productRepo.GetById(order.ProductId);
            

            _orderRepo.UpdateDeliveryConfirmed(order.OrderId, true);

            order.DeliveryConfirmed = true; // domain nesnesini de güncelle
            return order;
        }

        public void DeleteOrder(int id) => _orderRepo.Delete(id);
        public List<Order> GetAllOrders()
        {
            var orders = _orderRepo.GetAll();

            foreach (var order in orders)
            {
                order.Customer = _customerRepo.GetCustomerById(order.CustomerId);
                order.Product = _productRepo.GetById(order.ProductId);
            }

            return orders;
        }

        public Order GetOrderById(int id)
        {
            var order = _orderRepo.GetById(id);
            if (order == null)
                return null;

            // Navigation property'leri doldur
            order.Customer = _customerRepo.GetCustomerById(order.CustomerId);
            order.Product = _productRepo.GetById(order.ProductId);

            return order;
        }

    }
}

