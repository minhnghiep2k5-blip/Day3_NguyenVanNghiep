namespace Day3_NguyenVanNghiep.Models
{
    public class Data
    {
        public static List<Category> Categories = new List<Category>()
        {
            new Category() { Id = 1, Name = "Dien thoai" },
            new Category() { Id = 2, Name = "Laptop" },
            new Category() { Id = 3, Name = "Tablet" }
        };
        public static List<Product> Products = new List<Product>()
        {
            new Product() { Id = 1, Name = "Iphone 18 Pro Max", Price = 1000, Description = "Mau xanh dai duong", CategoryId = 1 },
            new Product() { Id = 2, Name = "Lenovo", Price = 1000, Description = "Nau la phong", CategoryId = 2 },
            new Product() { Id = 3, Name = "Ipad", Price = 1000, Description = "Den tram tinh", CategoryId = 3 }
        };

    }
}
