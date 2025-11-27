using Microsoft.AspNetCore.Mvc;
using SHOPDIENTU.Data;
using Microsoft.EntityFrameworkCore;

namespace SHOPDIENTU.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OrderController(ApplicationDbContext context) => _context = context;

        public IActionResult Index()
        {
            var orders = _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
            return View(orders);
        }
    }
}
