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
    }
}
