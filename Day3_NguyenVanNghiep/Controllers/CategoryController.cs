using Day3_NguyenVanNghiep.Helpers;
using Day3_NguyenVanNghiep.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace Day3_NguyenVanNghiep.Controllers
{
    public class CategoryController : Controller
    {
            // Mỗi trang hiển thị 2 danh mục
            private const int PageSize = 2;

            // Danh sách Category dùng chung
            public static List<Category> categories = new List<Category>
        {
            new Category { Id = 1, Name = "Điện thoại" },
            new Category { Id = 2, Name = "Laptop" },
            new Category { Id = 3, Name = "Phụ kiện" }
        };

            // GET: Category/Index
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

                // Lấy danh sách danh mục
                var query = categories.AsEnumerable();

                // 1. Tìm kiếm theo tên danh mục
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query = query.Where(c =>
                        !string.IsNullOrEmpty(c.Name) &&
                        c.Name.Contains(
                            searchTerm,
                            StringComparison.OrdinalIgnoreCase
                        )
                    );
                }

                // 2. Sắp xếp theo tên danh mục
                switch (sortOrder)
                {
                    // Sắp xếp Z -> A
                    case "name_desc":
                        query = query.OrderByDescending(
                            c => c.Name,
                            StringComparer.CurrentCultureIgnoreCase
                        );
                        break;

                    // Sắp xếp A -> Z
                    case "name_asc":
                        query = query.OrderBy(
                            c => c.Name,
                            StringComparer.CurrentCultureIgnoreCase
                        );
                        break;

                    // Mặc định sắp xếp theo ID
                    default:
                        query = query.OrderBy(c => c.Id);
                        break;
                }

                // 3. Tổng số danh mục sau tìm kiếm
                int totalCount = query.Count();

                // 4. Tính tổng số trang
                int totalPages = (int)Math.Ceiling(
                    (double)totalCount / PageSize
                );

                // Kiểm tra nếu trang vượt quá tổng số trang
                if (totalPages > 0 && pageNumber > totalPages)
                {
                    pageNumber = totalPages;
                }

                // 5. Lấy danh sách theo trang
                var items = query
                    .Skip((pageNumber - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();

            // 6. Tạo danh sách phân trang
                 var paginatedList = new PaginatedList<Category>(
                    items,
                    totalCount,
                    pageNumber,
                    PageSize
                    );

                return View("Index", paginatedList);
        }

            // GET: Category/Create
            public ActionResult Create()
            {
                return View();
            }

            // POST: Category/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Create(Category category)
            {
                if (ModelState.IsValid)
                {
                    // Tự động tăng ID
                    category.Id = categories.Any()
                        ? categories.Max(c => c.Id) + 1
                        : 1;

                    // Thêm danh mục
                    categories.Add(category);

                    return RedirectToAction(nameof(Index));
                }

                return View(category);
            }

            // GET: Category/Delete/5
            public ActionResult Delete(int id)
            {
                // Tìm danh mục theo ID
                var category = categories.FirstOrDefault(
                    c => c.Id == id
                );

                if (category == null)
                    return NotFound();

                // Kiểm tra danh mục có sản phẩm sử dụng không
                bool exists = ProductController.products.Any(
                    p => p.CategoryId == id
                );

                if (exists)
                {
                    return BadRequest(
                        "Không thể xóa danh mục vì đang có sản phẩm sử dụng!"
                    );
                }

                // Xóa danh mục
                categories.Remove(category);

                return RedirectToAction(nameof(Index));
            }
        }
    }
