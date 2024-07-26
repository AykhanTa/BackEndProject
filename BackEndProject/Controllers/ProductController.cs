using BackEndProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEndProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly JuanDbContext _juanDbContext;

        public ProductController(JuanDbContext juanDbContext)
        {
            _juanDbContext = juanDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ProductModal(int? id)
        {
            if (id == null) return BadRequest();
            var product = await _juanDbContext.Products
                .AsNoTracking()
                .Where(p=>!p.IsDelete)
                .Include(p=>p.ProductImages)
                .FirstOrDefaultAsync(p=> p.Id == id);
            if (product == null) return NotFound();
            return PartialView("_ModalPartial",product);

        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var product = await _juanDbContext.Products
                .AsNoTracking()
                .Where(p => !p.IsDelete)
                .Include(p => p.ProductImages)
                .Include(p=>p.ProductColors)
                .ThenInclude(pc=>pc.Color)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            var products = await _juanDbContext.Products
                .AsNoTracking()
                .Where(p => !p.IsDelete && p.Id!=product.Id)
                .Include(p => p.ProductImages)
                .Take(5)
                .ToListAsync();
            ViewBag.Products=products;
            return View(product);
        }
        public IActionResult SearchProduct(string search)
        {
            var products= _juanDbContext.Products
                .AsNoTracking()
                .Where(p=>!p.IsDelete&&p.Name.ToLower().Contains(search.ToLower()))
                .ToList();
            return PartialView("_SearchPartial",products);
        }
    }
}
