
namespace Day3_NguyenVanNghiep.Models
{
    public class ProductService
    {
        // đây là bài 5

        private readonly List<Product> _allProducts;

        public ProductService()
        {
            // Tạo dữ liệu giả
            _allProducts = Enumerable.Range(1, 100)
                .Select(i => new Product
                {
                    Id = i,
                    Name = $"Laptop {i}"
                })
                .ToList();
        }

        public (List<Product> Items, int TotalPages) Search(
            string keyword, int page, int pageSize)
        {
            var query = _allProducts.AsQueryable();

            // Tìm kiếm theo từ khóa
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(p =>
                    p.Name.Contains(keyword,
                    StringComparison.OrdinalIgnoreCase));
            }

            // Tính tổng số sản phẩm
            var total = query.Count();

            // Tính tổng số trang
            var totalPages = (int)Math.Ceiling(
                (double)total / pageSize);

            // Lấy sản phẩm theo trang
            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (items, totalPages);
        }
    }
}
