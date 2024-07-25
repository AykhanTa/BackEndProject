using System.ComponentModel.DataAnnotations;

namespace BackEndProject.Models
{
    public class Setting:BaseEntity
    {
        [Required]
        public string? Key { get; set; }
        [MaxLength(1000)]
        public string Value { get; set; }

    }
}
