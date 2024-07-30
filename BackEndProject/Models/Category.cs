using System.ComponentModel.DataAnnotations;

namespace BackEndProject.Models
{
    public class Category:BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public List<Product> Products { get; set; }

    }
}
