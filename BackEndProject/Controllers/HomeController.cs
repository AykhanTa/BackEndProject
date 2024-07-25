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
			return View(homeVM);
		}
 
		
	}
}
