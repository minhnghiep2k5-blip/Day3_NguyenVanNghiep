
using Day3_NguyenVanNghiep.Controllers;
using Day3_NguyenVanNghiep.Helpers;
using Day3_NguyenVanNghiep.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
public class ProductController : Controller
        {
            // Mỗi trang hiển thị 2 sản phẩm
            private const int PageSize = 2;

            // Danh sách sản phẩm
            public static List<Product> products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "iPhone 15",
                Price = 20000000,
                Description = "Điện thoại Apple",
                CategoryId = 1
            },
            new Product
            {
                Id = 2,
                Name = "Samsung S24",
                Price = 18000000,
                Description = "Điện thoại Samsung",
                CategoryId = 1
            },
            new Product
            {
                Id = 3,
                Name = "Dell Inspiron",
                Price = 15000000,
                Description = "Laptop Dell",
                CategoryId = 2
            }
        };

            // Kiểm tra Category có tồn tại không
            private bool CheckCategory(int categoryId)
            {
                return CategoryController.categories
                    .Any(c => c.Id == categoryId);
            }

            // Load danh sách Category
            private void LoadCategories(int? selectedId = null)
            {
                ViewBag.Categories = new SelectList(
                    CategoryController.categories,
                    "Id",
                    "Name",
                    selectedId
                );
            }

            // GET: Product/Index
            public ActionResult Index(
                string searchTerm,
                string sortOrder,
                int pageNumber = 1)
            {
                // Kiểm tra số trang
                if (pageNumber < 1)
                    pageNumber = 1;

                // Lưu thông tin tìm kiếm và sắp xếp
                ViewBag.SearchTerm = searchTerm;
                ViewBag.SortOrder = sortOrder;

                // Danh sách Category để hiển thị tên danh mục
                ViewBag.Categories = CategoryController.categories;

                // Lấy danh sách sản phẩm
                var query = products.AsEnumerable();

                // 1. Tìm kiếm theo tên hoặc mô tả
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query = query.Where(p =>
                        (!string.IsNullOrEmpty(p.Name) &&
                         p.Name.Contains(
                             searchTerm,
                             StringComparison.OrdinalIgnoreCase
                         ))
                        ||
                        (!string.IsNullOrEmpty(p.Description) &&
                         p.Description.Contains(
                             searchTerm,
                             StringComparison.OrdinalIgnoreCase
                         ))
                    );
                }

                // 2. Sắp xếp sản phẩm
                switch (sortOrder)
                {
                    // Tên Z -> A
                    case "name_desc":
                        query = query.OrderByDescending(
                            p => p.Name,
                            StringComparer.CurrentCultureIgnoreCase
                        );
                        break;

                    // Tên A -> Z
                    case "name_asc":
                        query = query.OrderBy(
                            p => p.Name,
                            StringComparer.CurrentCultureIgnoreCase
                        );
                        break;

                    // Giá cao -> thấp
                    case "price_desc":
                        query = query.OrderByDescending(p => p.Price);
                        break;

                    // Giá thấp -> cao
                    case "price_asc":
                        query = query.OrderBy(p => p.Price);
                        break;

                    // Mặc định theo ID
                    default:
                        query = query.OrderBy(p => p.Id);
                        break;
                }

                // 3. Tổng số sản phẩm sau tìm kiếm
                int totalCount = query.Count();

                // 4. Tính tổng số trang
                int totalPages = (int)Math.Ceiling(
                    (double)totalCount / PageSize
                );

                // Nếu số trang vượt quá tổng số trang
                if (totalPages > 0 && pageNumber > totalPages)
                {
                    pageNumber = totalPages;
                }

                // 5. Lấy sản phẩm theo trang
                var items = query
                    .Skip((pageNumber - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();

        // 6. Tạo danh sách phân trang

                 var paginatedList = new PaginatedList<Product>(
                    items,
                    totalCount,
                    pageNumber,
                    PageSize
);

                return View("Index", paginatedList);
    }

            // GET: Product/Create
            public ActionResult Create()
            {
                LoadCategories();
                return View();
            }

            // POST: Product/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Create(Product product)
            {
                // Kiểm tra Category
                if (!CheckCategory(product.CategoryId))
                {
                    ModelState.AddModelError(
                        "CategoryId",
                        "Mã Category không tồn tại! Vui lòng chọn Category có sẵn."
                    );
                }

                if (ModelState.IsValid)
                {
                    // Tự động tăng ID
                    product.Id = products.Any()
                        ? products.Max(p => p.Id) + 1
                        : 1;

                    products.Add(product);

                    return RedirectToAction(nameof(Index));
                }

                LoadCategories(product.CategoryId);
                return View(product);
            }

            // GET: Product/Details/5
            public ActionResult Details(int id)
            {
                var product = products.FirstOrDefault(p => p.Id == id);

                if (product == null)
                    return NotFound();

                ViewBag.CategoryName = CategoryController.categories
                    .FirstOrDefault(c => c.Id == product.CategoryId)?.Name;

                if (ViewBag.CategoryName == null)
                {
                    return NotFound(
                        "Không tìm thấy Category của sản phẩm!"
                    );
                }

                return View(product);
            }

            // GET: Product/Edit/5
            public ActionResult Edit(int id)
            {
                var product = products.FirstOrDefault(p => p.Id == id);

                if (product == null)
                    return NotFound();

                LoadCategories(product.CategoryId);

                return View(product);
            }

            // POST: Product/Edit/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Edit(int id, Product product)
            {
                if (id != product.Id)
                    return NotFound();

                var oldProduct = products.FirstOrDefault(p => p.Id == id);

                if (oldProduct == null)
                    return NotFound();

                // Kiểm tra Category
                if (!CheckCategory(product.CategoryId))
                {
                    ModelState.AddModelError(
                        "CategoryId",
                        "Mã Category không tồn tại!"
                    );
                }

                if (ModelState.IsValid)
                {
                    oldProduct.Name = product.Name;
                    oldProduct.Price = product.Price;
                    oldProduct.Description = product.Description;
                    oldProduct.CategoryId = product.CategoryId;

                    return RedirectToAction(nameof(Index));
                }

                LoadCategories(product.CategoryId);
                return View(product);
            }

            // GET: Product/Delete/5
            public ActionResult Delete(int id)
            {
                var product = products.FirstOrDefault(p => p.Id == id);

                if (product == null)
                    return NotFound();

                return View(product);
            }

            // POST: Product/Delete/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Delete(int id, IFormCollection collection)
            {
                var product = products.FirstOrDefault(p => p.Id == id);

                if (product == null)
                    return NotFound();

                products.Remove(product);

                return RedirectToAction(nameof(Index));
            }
        }