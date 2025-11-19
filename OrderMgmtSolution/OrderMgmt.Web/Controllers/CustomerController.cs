using Microsoft.AspNetCore.Mvc;
using OrderMgmt.Business;
using OrderMgmt.Domain;
using System.Collections.Generic;

namespace OrderMgmt.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerService _service;

        // DI ile CustomerService geliyor
        public CustomerController(CustomerService service)
        {
            _service = service;
        }


        // Listeleme
        public IActionResult Index()
        {
            List<Customer> customers = _service.GetAllCustomers();
            return View(customers);
        }

        // Yeni müşteri ekleme (GET)
        public IActionResult Add()
        {
            return View();
        }

        // Yeni müşteri ekleme (POST)
        [HttpPost]
        public IActionResult Add(Customer customer)
        {
            if (!ModelState.IsValid) return View(customer);
            _service.AddCustomer(customer);
            return RedirectToAction("Index");
        }

        // Düzenleme (GET)
        public IActionResult Edit(int id)
        {
            var customer = _service.GetCustomerById(id);
            if (customer == null) return NotFound();
            return View(customer);
        }

        // Düzenleme (POST)
        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            if (!ModelState.IsValid) return View(customer);
            _service.UpdateCustomer(customer);
            return RedirectToAction("Index");
        }

        // Silme (GET)
        public IActionResult Delete(int id)
        {
            var customer = _service.GetCustomerById(id);
            if (customer == null) return NotFound();
            return View(customer);
        }

        // Silme (POST)
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.DeleteCustomer(id);
            return RedirectToAction("Index");
        }
    }
}