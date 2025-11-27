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

        public IActionResult Index()
        {
            var products = _context.Products.ToList();

            var categories = _context.Products
                .Select(p => p.Category)
                .Where(c => !string.IsNullOrEmpty(c))
                .Distinct()
                .ToList();

            ViewBag.Categories = categories;

            return View(products);
        }
    }
}
