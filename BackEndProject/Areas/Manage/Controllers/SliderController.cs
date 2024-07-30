using BackEndProject.Data;
using BackEndProject.Extensions;
using BackEndProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace BackEndProject.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SliderController : Controller
    {
        private readonly JuanDbContext _juanDbContext;

        public SliderController(JuanDbContext juanDbContext)
        {
            _juanDbContext = juanDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var sliders= await _juanDbContext.Sliders
                .Where(s=>!s.IsDelete)
                .ToListAsync();
            return View(sliders);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (!ModelState.IsValid) return View(slider);
            var file = slider.Photo;
            if (file is null)
            {
                ModelState.AddModelError("Photo", "photo is required..");
                return View(slider);
            }
            if (!file.IsImage())
            {
                ModelState.AddModelError("Photo", "invalid file format..");
                return View(slider);
            }
            if (file.IsCorrectSize(100))
            {
                ModelState.AddModelError("Photo", "file size is too large..");
                return View(slider);
            }

            if (_juanDbContext.Sliders.Any(s=>!s.IsDelete && s.Title.ToLower()==slider.Title.ToLower()))
            {
                ModelState.AddModelError("Title", "title must be unique..");
                return View(slider);
            }
            slider.Title=slider.Title.Trim();
            slider.SubTitle = slider.SubTitle.Trim();
            slider.Image = await file.SaveFile("slider");
            slider.CreateDate = DateTime.Now;

            await _juanDbContext.Sliders.AddAsync(slider);
            await _juanDbContext.SaveChangesAsync();


            return RedirectToAction("index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            
            var slider= _juanDbContext.Sliders.FirstOrDefault(s=>s.Id==id);
            if (slider == null) return NotFound();
            slider.IsDelete=true;
            slider.DeleteDate = DateTime.Now;
            await _juanDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var slider = _juanDbContext.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider == null) return NotFound();

            return View(slider);
        }


        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var slider = _juanDbContext.Sliders.FirstOrDefault(s => s.Id == id);
            if (slider == null) return NotFound();
            return View(slider);
        }



        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, Slider slider)
        {
            if ((!ModelState.IsValid)) return View(slider);
            if (id == null) return BadRequest();
            if (id != slider.Id) return BadRequest();
            var existSlider = _juanDbContext.Sliders.FirstOrDefault(s => s.Id == id);
            if (existSlider == null) return NotFound();
            var file = slider.Photo;
            if (file != null)
            {
                if (!file.IsImage())
                {
                    ModelState.AddModelError("Photo", "invalid file format..");
                    return View(slider);
                }
                if (file.IsCorrectSize(100))
                {
                    ModelState.AddModelError("Photo", "file size is too large..");
                    return View(slider);
                }

                if (_juanDbContext.Sliders.Any(s => !s.IsDelete && s.Title.ToLower() == slider.Title.ToLower()))
                {
                    ModelState.AddModelError("Title", "title must be unique..");
                    return View(slider);
                }
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "img", "slider", existSlider.Image);

                existSlider.Image = await file.SaveFile("slider");
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            existSlider.Title = slider.Title.Trim();
            existSlider.SubTitle=slider.SubTitle.Trim();
            existSlider.Description = slider.Description.Trim();
            existSlider.UpdateDate = DateTime.Now;
            await _juanDbContext.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
