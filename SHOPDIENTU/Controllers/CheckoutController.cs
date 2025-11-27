using Microsoft.AspNetCore.Mvc;
using SHOPDIENTU.Data;
using SHOPDIENTU.Models;
using SHOPDIENTU.Services;

namespace SHOPDIENTU.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CheckoutController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Info()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            if (!cart.Any()) return RedirectToAction("Index", "Cart");

            ViewBag.Cart = cart;
            return View();
        }

        [HttpPost]
        public IActionResult Info(CheckoutInfo order)
        {
            HttpContext.Session.SetObjectAsJson("UserOrder", order);
            return RedirectToAction("Payment");
        }

        [HttpGet]
        public IActionResult Payment()
        {
            var order = HttpContext.Session.GetObjectFromJson<UserOrder>("UserOrder") ?? new UserOrder();
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart") ?? new();

            decimal subtotal = cart.Sum(i => i.Product.Price * i.Quantity);
            decimal total = subtotal;

            ViewBag.SubTotal = subtotal;
            ViewBag.TotalItems = cart.Sum(i => i.Quantity);
            ViewBag.Total = total;

            return View(order);
        }

        public IActionResult Pay(string method)
        {
            var order = HttpContext.Session.GetObjectFromJson<UserOrder>("UserOrder");

            // ⭐ THÊM DÒNG NÀY – BẮT BUỘC
            order.Address = order.Address;

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");

            if (order == null || cart == null)
                return RedirectToAction("Info");

            order.PaymentMethod = method;
            HttpContext.Session.SetObjectAsJson("UserOrder", order);

            long amount = Convert.ToInt64(cart.Sum(i => i.Product.Price * i.Quantity));

            if (method == "cod")
            {
                return RedirectToAction("ConfirmOrder");
            }

            if (method == "bank")
            {
                return RedirectToAction("BankTransfer");
            }

            if (method == "momo")
            {
                var momo = new MomoService();
                string url = momo.CreatePayment(Guid.NewGuid().ToString(), amount);
                return Redirect(url);
            }

            return RedirectToAction("Payment");
        }



        public IActionResult ConfirmOrder()
        {
            var orderInfo = HttpContext.Session.GetObjectFromJson<CheckoutInfo>("UserOrder");
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");

            if (orderInfo == null || cart == null)
                return RedirectToAction("Index", "Cart");

            var dbOrder = new Order
            {
                CustomerName = orderInfo.FullName,
                Phone = orderInfo.Phone,
                Address = orderInfo.Address,
                PaymentMethod = orderInfo.PaymentMethod,
                TotalAmount = cart.Sum(i => i.Product.Price * i.Quantity),
                Items = cart.Select(c => new OrderItem
                {
                    ProductId = c.Product.Id,
                    Quantity = c.Quantity,
                    Price = c.Product.Price
                }).ToList()
            };

            _context.Orders.Add(dbOrder);
            _context.SaveChanges();
            HttpContext.Session.Remove("Cart");

            return RedirectToAction("Success");
        }



        public IActionResult Success()
        {
            return View();
        }
    }
}
