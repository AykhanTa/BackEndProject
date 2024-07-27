using BackEndProject.Data;
using BackEndProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEndProject.Controllers
{
	public class HomeController : Controller
	{
		private readonly JuanDbContext _context;

        public HomeController(JuanDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
		{
			HomeVM homeVM = new();
			homeVM.Sliders=await _context.Sliders
				.AsNoTracking()
				.Where(s=>!s.IsDelete)
				.ToListAsync();
			homeVM.Products=await _context.Products
				.AsNoTracking()
				.Where (p=>!p.IsDelete)
				.ToListAsync();
			homeVM.Blogs=await _context.Blogs
				.AsNoTracking()
				.Where(b=>!b.IsDelete)
				.ToListAsync();
			homeVM.Banners = await _context.Banners
				.AsNoTracking()
				.Where(b => !b.IsDelete)
				.ToListAsync();
			ViewBag.BannerPhoto = _context.Settings.FirstOrDefault(s=>!s.IsDelete&&s.Key=="BannerPhoto").Value;
			return View(homeVM);
		}
 
		
	}
}
