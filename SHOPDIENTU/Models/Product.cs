using System.ComponentModel.DataAnnotations.Schema;

namespace SHOPDIENTU.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }

        public string? Image { get; set; }
        public string? Category { get; set; }
        public string? Brand { get; set; }
        public string? Description { get; set; }
   

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public int Discount { get; set; } 
    }
}
