using Day3_NguyenVanNghiep.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Day3_NguyenVanNghiep.Controllers
{
    public class CategoryController : Controller
    {
        // gia ap csdl
        private static List<Category> _categories = new List<Category>()
        {
            new Category() { Id = 1, Name = "Iphone 18 Pro Max" },
            new Category() { Id = 2, Name = "Iphone 18 Pro Max" },
            new Category() { Id = 3, Name = "Iphone 18 Pro Max" }
        };
        // GET: CategoryController
        public ActionResult Index()
        {
            return View(_categories);
        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            var category = _categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // GET: CategoryController/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] Category category)
        {
            if (ModelState.IsValid)
            {
                // Thêm category vào danh sách _categories
                // Tạo id mới cho category = max id hiện tại + 1
                int newId = _categories.Any() ? _categories.Max(c => c.Id) + 1 : 1;
                _categories.Add(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: CategoryController/Edit/5
        public ActionResult Edit([FromRoute]int id)
        {
            var category = _categories.FirstOrDefault(c => c.Id == id);
            // neu khong tim thay category theo id thi tra ve notfound
            if (category == null)
            {
                return NotFound();
            }
            // neu tim thay category thi tra ve view edit va truyen category vao view
            return View(category);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            // tim category theo id
            var existingCategory = _categories.FirstOrDefault(c => c.Id == category.Id);
           if (existingCategory == null) {
                return NotFound();
            }
           if (ModelState.IsValid)
            {
                // cap nhat category
                existingCategory.Name = category.Name;
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: CategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            var category = _categories.FirstOrDefault(c => c.Id == id); 
            if (category == null) {
                return NotFound();
            }
            return View(category);
        }

        // POST: CategoryController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var category = _categories.FirstOrDefault(c => c.Id == id);
            if (category == null){
                    return NotFound();
            }
             _categories.Remove(category);
             return RedirectToAction(nameof(Index));
        }
    }
}
