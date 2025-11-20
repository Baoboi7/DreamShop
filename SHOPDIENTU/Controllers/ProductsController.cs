using Microsoft.AspNetCore.Mvc;
using SHOPDIENTU.Data;
using SHOPDIENTU.Models;
using System.Linq;

namespace SHOPDIENTU.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🧩 Hiển thị sản phẩm theo danh mục / hãng / giá
        public IActionResult Category(string category, string brand, string priceRange)
        {
            // ✅ Lấy danh sách sản phẩm từ DB
            var products = _context.Products.AsQueryable();

            // 🏷️ Lọc theo danh mục (Category)
            if (!string.IsNullOrEmpty(category))
                products = products.Where(p => p.Category.ToLower().Trim() == category.ToLower().Trim());

            // 🔍 Lọc theo hãng (Brand)
            if (!string.IsNullOrEmpty(brand))
                products = products.Where(p => p.Brand.ToLower().Trim() == brand.ToLower().Trim());

            // 💰 Lọc theo khoảng giá
            if (!string.IsNullOrEmpty(priceRange))
            {
                switch (priceRange)
                {
                    case "duoi-10tr":
                        products = products.Where(p => p.Price < 10000000);
                        break;
                    case "10-20tr":
                        products = products.Where(p => p.Price >= 10000000 && p.Price <= 20000000);
                        break;
                    case "tren-20tr":
                        products = products.Where(p => p.Price > 20000000);
                        break;
                }
            }

            // ✅ Lấy danh sách hãng riêng theo danh mục đang xem
            List<string> brands = new List<string>();

            if (!string.IsNullOrEmpty(category))
            {
                brands = _context.Products
                    .Where(p => p.Category.ToLower().Trim() == category.ToLower().Trim() && !string.IsNullOrEmpty(p.Brand))
                    .Select(p => p.Brand)
                    .Distinct()
                    .ToList();
            }
            else
            {
                // Nếu chưa chọn danh mục nào, lấy tất cả hãng
                brands = _context.Products
                    .Where(p => !string.IsNullOrEmpty(p.Brand))
                    .Select(p => p.Brand)
                    .Distinct()
                    .ToList();
            }

            // ✅ Truyền dữ liệu sang View
            ViewBag.Brands = brands;
            ViewBag.Category = category;

            return View(products.ToList());
        }

        // 📄 Trang chi tiết sản phẩm
        public IActionResult Details(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            // 🛍️ Lấy sản phẩm cùng danh mục (liên quan)
            var related = _context.Products
                .Where(p => p.Category == product.Category && p.Id != product.Id)
                .Take(4)
                .ToList();

            ViewBag.Related = related;

            return View(product);
        }
    }
}
