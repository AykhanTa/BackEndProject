using System.ComponentModel.DataAnnotations.Schema;

namespace BackEndProject.Models
{
    public class Slider:BaseEntity
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
