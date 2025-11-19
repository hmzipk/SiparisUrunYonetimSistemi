using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrderMgmt.Business;
using OrderMgmt.DataAccess;
using OrderMgmt.Domain;

namespace OrderMgmt.Web.Controllers
{
    public class OrdersController : Controller
    {
        /* Servis ve depo bağımlılıklar
         Kod güvenliği: Yanlışlıkla başka bir yerde servisi değiştirmemek için "readonly" kullanıldı.*/
        private readonly OrderService _orderService;
        private readonly CustomerRepository _customerRepo;
        private readonly ProductRepository _productRepo;

        // DI ile bağımlılıklar geliyor
        public OrdersController(OrderService orderService,
                                CustomerRepository customerRepo,
                                ProductRepository productRepo)
        {
            _orderService = orderService;
            _customerRepo = customerRepo;
            _productRepo = productRepo;
        }


        // Sipariş listesi
        public IActionResult Index()
        {
            var orders = _orderService.GetAllOrders();
            return View("Index", orders); 
        }

        // Sipariş oluşturma formu (GET)
        [HttpGet]
        public IActionResult Add()
        {
            var products = _productRepo.GetAll();
            ViewBag.Products = new SelectList(products, "ProductId", "StockName");

            var productPrices = new Dictionary<int, decimal>();
            foreach (var p in products)
            {
                productPrices[p.ProductId] = p.Price;
            }
            ViewBag.ProductPrices = productPrices;

            return View(new Order());
        }

        // Sipariş oluşturma (POST)
        [HttpPost]
        public IActionResult Add(Order order, string CustomerEmail)
        {
            var products = _productRepo.GetAll();
            ViewBag.Products = new SelectList(products, "ProductId", "StockName");

            var customer = _customerRepo.GetByEmail(CustomerEmail);
            if (customer == null)
            {
                ModelState.AddModelError("", "Müşteri bulunamadı.");
                return View(order);
            }

            order.CustomerId = customer.CustomerId;

            try
            {
                _orderService.AddOrder(order);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Sipariş eklenemedi: {ex.Message}");
                return View(order);
            }
        }

        // GET: Sipariş silme onay sayfası
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var order = _orderService.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order); // Delete.cshtml
        }

        // POST: Sipariş silme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Order order)
        {
            _orderService.DeleteOrder(order.OrderId);
            return RedirectToAction("Index");
        }




        // Teslim onayı
        [HttpGet]
        public IActionResult ConfirmDelivery(string token)
        {
            try
            {
                var order = _orderService.ConfirmDelivery(token);
                return View("DeliveryConfirmed", order); // Model gönderiliyor
            }
            catch
            {
                return View("InvalidToken");
            }
        }

    }
}

