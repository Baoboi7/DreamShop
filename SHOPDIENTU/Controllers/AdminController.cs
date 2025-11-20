using Microsoft.AspNetCore.Mvc;
using SHOPDIENTU.Data;
using SHOPDIENTU.Models;

namespace SHOPDIENTU.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AdminController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ==================== QUẢN LÝ SẢN PHẨM ====================
        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            var users = _context.Users.ToList(); // 🆕 Thêm user vào view model
            ViewBag.Users = users;               // 🆕 Gửi qua view
            return View(products);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Product product, IFormFile? ImageFile)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                string uploadPath = Path.Combine(_env.WebRootPath, "img");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                string fileName = Path.GetFileName(ImageFile.FileName);
                string fullPath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }
                product.Image = "/img/" + fileName;
            }

            if (product.Price <= 0)
                ModelState.AddModelError("Price", "Giá sản phẩm phải lớn hơn 0.");

            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product, IFormFile? ImageFile)
        {
            var existing = _context.Products.Find(product.Id);
            if (existing == null) return NotFound();

            if (ImageFile != null && ImageFile.Length > 0)
            {
                string uploadPath = Path.Combine(_env.WebRootPath, "img");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                string fileName = Path.GetFileName(ImageFile.FileName);
                string fullPath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }
                existing.Image = "/img/" + fileName;
            }

            existing.Name = product.Name;
            existing.Brand = product.Brand;
            existing.Category = product.Category;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.Discount = product.Discount;

            _context.Products.Update(existing);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // ==================== 🧩 QUẢN LÝ NGƯỜI DÙNG ====================
        // 🆕 Xem danh sách user ngay trên tab "Người dùng"
        [HttpGet]
        public IActionResult GetUsersPartial()
        {
            var users = _context.Users.ToList();
            return PartialView("_UserListPartial", users);
        }

        // 🆕 Xóa tài khoản người dùng
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
