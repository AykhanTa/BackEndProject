using BackEndProject.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEndProject.Data
{
    public class JuanDbContext : DbContext
    {
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public JuanDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
