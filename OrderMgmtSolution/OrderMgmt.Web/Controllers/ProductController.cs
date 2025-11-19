using Microsoft.AspNetCore.Mvc;
using OrderMgmt.Business;
using OrderMgmt.Domain;

namespace OrderMgmt.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _service;

        public ProductController(ProductService service)
        {
            _service = service;
        }



        // Listeleme
        public IActionResult Index()
        {
            var products = _service.GetAllProducts();
            return View(products);
        }
        
        // Yeni ürün ekleme (GET)
        public IActionResult Add()
        {
            return View();
        }
        
        // Yeni ürün ekleme (POST)
        [HttpPost]
        public IActionResult Add(Product product)
        {
            if (ModelState.IsValid)
            {
                _service.AddProduct(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // Ürün düzenleme (GET)
        public IActionResult Edit(int id)
        {
            var product = _service.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // Ürün düzenleme (POST)
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _service.UpdateProduct(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // Ürün silme (GET)
        public IActionResult Delete(int id)
        {
            var product = _service.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // Ürün silme (POST)
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _service.GetProductById(id);
            if (product != null)
            {
                _service.DeleteProduct(product.ProductId); // Artık int gönderiyoruz
            }
            return RedirectToAction("Index");
        }

    }
}