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

        public IActionResult Category(string category, string brand, string priceRange)
        {
            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(category))
                products = products.Where(p => p.Category.ToLower().Trim() == category.ToLower().Trim());

            if (!string.IsNullOrEmpty(brand))
                products = products.Where(p => p.Brand.ToLower().Trim() == brand.ToLower().Trim());

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
                brands = _context.Products
                    .Where(p => !string.IsNullOrEmpty(p.Brand))
                    .Select(p => p.Brand)
                    .Distinct()
                    .ToList();
            }

            ViewBag.Brands = brands;
            ViewBag.Category = category;

            return View(products.ToList());
        }

        public IActionResult Details(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();

            var related = _context.Products
                .Where(p => p.Category == product.Category && p.Id != product.Id)
                .Take(4)
                .ToList();

            ViewBag.Related = related;

            return View(product);
        }
      public IActionResult Search(string keyword)
{
    if (string.IsNullOrEmpty(keyword))
    {
        return View("Category", _context.Products.ToList());
    }

    var result = _context.Products
        .Where(p => p.Name.ToLower().Contains(keyword.ToLower())
                 || p.Brand.ToLower().Contains(keyword.ToLower()))
        .ToList();

    return View("Category", result);
}


    }
}
