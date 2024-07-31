using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndProject.Models
{
    public class Product:BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public int Count { get; set; }
        public string MainImage { get; set; }
        [NotMapped]
        public IFormFile MainPhoto { get; set; }
        [Column(TypeName = "money")]
        public decimal Price { get; set; }
        [Column(TypeName = "money")]
        public decimal DiscountPrice { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ExTax { get; set; }
        public bool IsNewProduct { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        [NotMapped]
        public IFormFile[] Photos { get; set; }
        [NotMapped]
        public IEnumerable<int> ColorIds { get; set; }
        [NotMapped]
        public IEnumerable<int> SizeIds { get; set; }
        public List<ProductColor> ProductColors { get; set; }
        public List<ProductSize> ProductSizes { get; set; }
    }
}
