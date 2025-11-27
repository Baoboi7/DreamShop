using Microsoft.AspNetCore.Mvc;
using SHOPDIENTU.Data;
using SHOPDIENTU.Models;

namespace SHOPDIENTU.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

  
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart")
                       ?? new List<CartItem>();
            return View(cart);
        }

      
        [HttpPost]
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var product = _context.Products.Find(id);
            if (product == null)
                return NotFound();

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart")
                       ?? new List<CartItem>();

            var existing = cart.FirstOrDefault(p => p.Product.Id == id);
            if (existing != null)
                existing.Quantity += quantity;
            else
                cart.Add(new CartItem { Product = product, Quantity = quantity });

            HttpContext.Session.SetObjectAsJson("Cart", cart);

            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult BuyNow(int id, int quantity = 1)
        {
            var product = _context.Products.Find(id);
            if (product == null)
                return NotFound();

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart")
                       ?? new List<CartItem>();

            var existing = cart.FirstOrDefault(p => p.Product.Id == id);
            if (existing != null)
                existing.Quantity += quantity;
            else
                cart.Add(new CartItem { Product = product, Quantity = quantity });

            HttpContext.Session.SetObjectAsJson("Cart", cart);

            return RedirectToAction("Info", "Checkout");
        }


        public IActionResult Remove(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");

            if (cart != null)
            {
                cart.RemoveAll(p => p.Product.Id == id);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }

            return RedirectToAction("Index");
        }
    
        public IActionResult Checkout()
        {
            return RedirectToAction("Info", "Checkout");
        }
    }
}
