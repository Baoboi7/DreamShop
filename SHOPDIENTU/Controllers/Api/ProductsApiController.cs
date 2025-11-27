using Microsoft.AspNetCore.Mvc;
using SHOPDIENTU.Data;
using SHOPDIENTU.Models;

namespace SHOPDIENTU.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ProductsApiController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/productsapi
        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _db.Products.ToList();
            return Ok(products); // trả về JSON
        }

        // GET: api/productsapi/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _db.Products.Find(id);
            if (product == null)
                return NotFound(new { message = "Không tìm thấy sản phẩm" });

            return Ok(product);
        }

        // POST: api/productsapi
        [HttpPost]
        public IActionResult Create([FromBody] Product model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _db.Products.Add(model);
            _db.SaveChanges();

            return Ok(model);
        }

        // PUT: api/productsapi/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Product model)
        {
            var p = _db.Products.Find(id);
            if (p == null)
                return NotFound();

            p.Name = model.Name;
            p.Price = model.Price;
            p.Image = model.Image;
            p.Description = model.Description;

            _db.SaveChanges();

            return Ok(p);
        }

        // DELETE: api/productsapi/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var p = _db.Products.Find(id);
            if (p == null)
                return NotFound();

            _db.Products.Remove(p);
            _db.SaveChanges();
            return Ok(new { message = "Đã xoá thành công" });
        }
    }
}
