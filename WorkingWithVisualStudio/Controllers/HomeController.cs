using Microsoft.AspNetCore.Mvc;
using WorkingWithVisualStudio.Models;
using System.Linq;

namespace WorkingWithVisualStudio.Controllers {
    public class HomeController : Controller {
        public IRepository Repository = SimpleRepository.SharedRepository;

        // added . Where with price parameter
        //public IActionResult Index() => View(Repository.Products.Where(p => p?.Price < 50));
        // now with no more  Where
        public IActionResult Index() => View(Repository.Products);

        [HttpGet]
        public IActionResult AddProduct() => View(new Product());

        [HttpPost]
        public IActionResult AddProduct(Product p) {
            Repository.AddProduct(p);
            return RedirectToAction("Index");
        }
    }
}
