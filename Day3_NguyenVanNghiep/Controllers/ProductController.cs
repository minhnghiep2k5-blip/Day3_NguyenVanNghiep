
using Day3_NguyenVanNghiep.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Day3_NguyenVanNghiep.Controllers
{
    public class ProductController : Controller
    {
        private static List<Product> _products = new List<Product>()
    {
        new Product() { Id = 1, Name = "Iphone 18 Pro Max", Price = 1000, Description = "Mau xanh dai duong", CategoryId = 1 },
        new Product() { Id = 2, Name = "Iphone 18 Pro Max", Price = 1000, Description = "Nau la phong", CategoryId = 2 },
        new Product() { Id = 3, Name = "Iphone 18 Pro Max", Price = 1000, Description = "Den tram tinh", CategoryId = 3 }
    };
        // GET: ProductController1
        public ActionResult Index()
        {
            return View(_products);
        }

        // GET: ProductController1/Details/5
        public ActionResult Details(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: ProductController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] Product product)
        {
            if (ModelState.IsValid)
            {
                // Thêm product vào danh sách _products
                // Tạo id mới cho product = max id hiện tại + 1
                int newId = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
                _products.Add(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: ProductController1/Edit/5
        public ActionResult Edit(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) {
                return NotFound();
            }
            return View(product);
        }

        // POST: ProductController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
          var existingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
          if (existingProduct == null) {
                return NotFound();
           }
          if (ModelState.IsValid)
            {
                // Cập nhật thông tin product
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: ProductController1/Delete/5
        public ActionResult Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) {
                return NotFound();
            }
            return View(product);
        }

        // POST: ProductController1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null) {
                return NotFound();
            }
            _products.Remove(product);
            return RedirectToAction(nameof(Index));
        }
    }
}