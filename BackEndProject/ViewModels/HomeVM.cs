using BackEndProject.Models;

namespace BackEndProject.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public List<Product> Products { get; set; }
        public List<Blog> Blogs { get; set; }
    }
}
