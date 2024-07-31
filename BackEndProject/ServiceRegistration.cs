using BackEndProject.Data;
using BackEndProject.Interfaces;
using BackEndProject.Models;
using BackEndProject.Services;
using Microsoft.AspNetCore.Identity;
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

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 7;

                options.User.RequireUniqueEmail=true;

                options.Lockout.AllowedForNewUsers=true;
                options.Lockout.MaxFailedAccessAttempts=3;
                options.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromMinutes(10);
            }).AddEntityFrameworkStores<JuanDbContext>()
            .AddDefaultTokenProviders();

        }
    }
}
