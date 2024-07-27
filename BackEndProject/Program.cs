using BackEndProject.Data;
using BackEndProject.Interfaces;
using BackEndProject.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config=builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<JuanDbContext>(options =>
{
	options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<ILayoutService,LayoutService>();
builder.Services.AddSession(options =>
{
	options.IdleTimeout=TimeSpan.FromMinutes(10);
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapDefaultControllerRoute();
//app.MapControllerRoute(
//	name: "default",
//	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
