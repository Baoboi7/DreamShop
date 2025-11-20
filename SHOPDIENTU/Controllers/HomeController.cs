using Microsoft.AspNetCore.Mvc;
using SHOPDIENTU.Data;
using System.Linq;

namespace SHOPDIENTU.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Trang chủ tự động lấy danh mục + sản phẩm
        public IActionResult Index()
        {
            // Lấy toàn bộ sản phẩm để hiển thị
            var products = _context.Products.ToList();

            // Lấy danh sách Category duy nhất (Distinct)
            var categories = _context.Products
                .Select(p => p.Category)
                .Where(c => !string.IsNullOrEmpty(c))
                .Distinct()
                .ToList();

            // Truyền danh sách Category sang View
            ViewBag.Categories = categories;

            return View(products);
        }
    }
}
