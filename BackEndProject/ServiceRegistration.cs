using BackEndProject.Data;
using BackEndProject.Interfaces;
using BackEndProject.Services;
using Microsoft.EntityFrameworkCore;

namespace BackEndProject
{
    public static class ServiceRegistration
    {
        public static void Register(this IServiceCollection services,IConfiguration config)
        {
            services.AddControllersWithViews();

            services.AddDbContext<JuanDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<ILayoutService, LayoutService>();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
            });

            services.AddHttpContextAccessor();


        }
    }
}
