using SHOPDIENTU.Models;

namespace SHOPDIENTU.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Products.Any())
            {
                return; // Dữ liệu đã tồn tại
            }

            var products = new Product[]
            {
                new Product { Name = "iPhone 15 Pro Max", Price = 33990000, Image = "/img/img1.jpg", Category = "Phone", Brand = "Apple", Description = "Siêu phẩm Apple 2024" },
                new Product { Name = "Samsung Galaxy S24 Ultra", Price = 28990000, Image = "/img/img2.jpg", Category = "Phone", Brand = "Samsung", Description = "Màn hình AMOLED, zoom 100x" },
                new Product { Name = "OPPO Reno 11 5G", Price = 12990000, Image = "/img/img3.jpg", Category = "Phone", Brand = "OPPO", Description = "Hiệu năng mạnh, camera đẹp" },
                new Product { Name = "ASUS ROG Laptop", Price = 24990000, Image = "/img/img4.jpg", Category = "Laptop", Brand = "ASUS", Description = "Hiệu năng gaming cực khủng" }
            };

            foreach (Product p in products)
            {
                context.Products.Add(p);
            }

            context.SaveChanges();
        }
    }
}
