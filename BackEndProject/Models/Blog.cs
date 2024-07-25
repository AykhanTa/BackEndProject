namespace BackEndProject.Models
{
    public class Blog:BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public List<BlogTag> BlogTags { get; set; }

    }
}
