using BackEndProject.Data;
using BackEndProject.Extensions;
using BackEndProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEndProject.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class CategoryController : Controller
    {
        private readonly JuanDbContext _context;

        public CategoryController(JuanDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories= await _context.Categories
                .AsNoTracking()
                .Where(c=>!c.IsDelete)
                .ToListAsync();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid) return View(category);
            
            if (_context.Categories.Any(s => !s.IsDelete && s.Name.ToLower() == category.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Category name must be unique..");
                return View(category);
            }
            category.Name = category.Name.Trim();
            category.CreateDate = DateTime.Now;

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();


            return RedirectToAction("index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            var category = _context.Categories.FirstOrDefault(s => s.Id == id);
            if (category == null) return NotFound();
            category.IsDelete = true;
            category.DeleteDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var category = _context.Categories
                .AsNoTracking()
                .Include(category => category.Products)
                .FirstOrDefault(s => s.Id == id);
            if (category == null) return NotFound();

            return View(category);
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var category = _context.Categories.AsNoTracking().FirstOrDefault(s => s.Id == id);
            if (category == null) return NotFound();
            return View(category);
        }



        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, Category category)
        {
            if ((!ModelState.IsValid)) return View(category);
            if (id == null) return BadRequest();
            if (id != category.Id) return BadRequest();
            var existCategory = _context.Categories.FirstOrDefault(s => s.Id == id);
            if (existCategory == null) return NotFound();
            

                if (_context.Categories.Any(c => !c.IsDelete && c.Name.ToLower() == category.Name.ToLower()))
                {
                    ModelState.AddModelError("Name", "Category name must be unique..");
                    return View(category);
                }
            existCategory.Name = category.Name.Trim();
            existCategory.UpdateDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
