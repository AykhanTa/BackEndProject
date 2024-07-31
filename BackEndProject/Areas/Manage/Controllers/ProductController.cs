using BackEndProject.Data;
using BackEndProject.Extensions;
using BackEndProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BackEndProject.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProductController : Controller
    {
        private readonly JuanDbContext _juanDbContext;

        public ProductController(JuanDbContext juanDbContext)
        {
            _juanDbContext = juanDbContext;
        }

        public IActionResult Index()
        {
            var products = _juanDbContext.Products
                .AsNoTracking()
                .Where(p => !p.IsDelete)
                .ToList();
            return View(products);
        }
        public IActionResult Detail(int? id)
        {
            if (id == null) return BadRequest();
            var existProduct = _juanDbContext.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .FirstOrDefault(p => !p.IsDelete && p.Id == id);
            if (existProduct == null) return NotFound();
            return View(existProduct);
        }
        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id == null) return BadRequest();

            var productImage = await _juanDbContext.ProductImages.FirstOrDefaultAsync(p => p.Id == id && !p.IsDelete);
            if (productImage == null) return NotFound();
            productImage.IsDelete = true;
            productImage.DeleteDate = DateTime.Now;

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "img", "product", productImage.Name);
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);

            await _juanDbContext.SaveChangesAsync();

            return RedirectToAction("detail", new { id = productImage.ProductId });

        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var existProduct =await _juanDbContext.Products
                .FirstOrDefaultAsync(p => !p.IsDelete && p.Id == id);
            if (existProduct == null) return NotFound();
            existProduct.IsDelete = true;
            existProduct.DeleteDate=DateTime.Now;
            await _juanDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _juanDbContext.Categories
                .AsNoTracking()
                .Where(c => !c.IsDelete)
                .ToListAsync();
            ViewBag.Colors = await _juanDbContext.Colors
                .AsNoTracking()
                .Where(co => !co.IsDelete)
                .ToListAsync();
            ViewBag.Sizes = await _juanDbContext.Sizes
                .AsNoTracking()
                .Where(s => !s.IsDelete)
                .ToListAsync();

            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.Categories = await _juanDbContext.Categories
                .AsNoTracking()
                .Where(c => !c.IsDelete)
                .ToListAsync();
            ViewBag.Colors = await _juanDbContext.Colors
                .AsNoTracking()
                .Where(co => !co.IsDelete)
                .ToListAsync();
            ViewBag.Sizes = await _juanDbContext.Sizes
                .AsNoTracking()
                .Where(s => !s.IsDelete)
                .ToListAsync();

            if (!ModelState.IsValid) return View(product);
            if (!await _juanDbContext.Categories.Where(c => !c.IsDelete).AnyAsync(c => c.Id == product.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "categoryId is not correct...");
                return View(product);

            }
            Product newProduct = new();

            List<ProductColor> colors = new();
            foreach (var colorId in product.ColorIds)
            {
                if (!await _juanDbContext.Colors.Where(c => !c.IsDelete).AnyAsync(c => c.Id == colorId))
                {
                    ModelState.AddModelError("ColorIds", "colorIds are not correct...");
                    return View(product);
                }
                ProductColor productColor = new();
                productColor.ColorId = colorId;
                productColor.ProductId = newProduct.Id;
                colors.Add(productColor);
            }


            List<ProductSize> sizes = new();
            foreach (var sizeId in product.SizeIds)
            {
                if (!await _juanDbContext.Sizes.Where(s => !s.IsDelete).AnyAsync(s => s.Id == sizeId))
                {
                    ModelState.AddModelError("SizeIds", "sizeIds are not correct...");
                    return View(product);
                }
                ProductSize productSize = new();
                productSize.Sizeid = sizeId;
                productSize.ProductId = newProduct.Id;
                sizes.Add(productSize);

            }

            var mainFile = product.MainPhoto;
            var files = product.Photos;

            if (mainFile == null || files == null)
            {
                ModelState.AddModelError("", "photos is required...");
                return View(product);
            }
            if (!mainFile.IsImage())
            {
                ModelState.AddModelError("MainPhoto", "invalid file format..");
                return View(product);
            }
            if (mainFile.IsCorrectSize(100))
            {
                ModelState.AddModelError("MainPhoto", "file size is too large..");
                return View(product);
            }

            newProduct.MainImage = await mainFile.SaveFile("product");


            List<ProductImage> list = new();
            foreach (var photo in files)
            {
                if (!photo.IsImage())
                {
                    ModelState.AddModelError("Photos", "invalid file format..");
                    return View(product);
                }
                if (photo.IsCorrectSize(100))
                {
                    ModelState.AddModelError("Photos", "file size is too large..");
                    return View(product);
                }
                ProductImage productImage = new();
                productImage.ProductId = product.Id;
                productImage.CreateDate = DateTime.Now;
                productImage.Name = await photo.SaveFile("product");
                list.Add(productImage);

            }


            newProduct.Name = product.Name.Trim();
            newProduct.Description = product.Description;
            newProduct.CategoryId = product.CategoryId;
            newProduct.Price = product.Price;
            newProduct.ExTax = product.ExTax;
            newProduct.Count = product.Count;
            newProduct.ProductImages = list;
            newProduct.ProductColors = colors;
            newProduct.ProductSizes = sizes;
            newProduct.IsNewProduct = true;
            newProduct.CreateDate = DateTime.Now;

            await _juanDbContext.Products.AddAsync(newProduct);
            await _juanDbContext.SaveChangesAsync();


            return View();
        }



        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var product = await _juanDbContext.Products
                .Include(p=>p.ProductColors)
                .Include(p=>p.ProductSizes)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDelete);
            if (product == null) return NotFound();
            ViewBag.Categories = await _juanDbContext.Categories.Where(c => !c.IsDelete).ToListAsync();
            ViewBag.Colors = await _juanDbContext.Colors.Where(c => !c.IsDelete).ToListAsync();
            ViewBag.Sizes = await _juanDbContext.Sizes.Where(s => !s.IsDelete).ToListAsync();
            return View(product);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id,Product product)
        {
            if (id == null) return BadRequest();
            var existProduct = await _juanDbContext.Products
                .Include(p=>p.ProductColors)
                .Include(p=>p.ProductSizes)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDelete);
            if (existProduct == null) return NotFound();
            ViewBag.Categories = await _juanDbContext.Categories.Where(c => !c.IsDelete).ToListAsync();
            ViewBag.Colors = await _juanDbContext.Colors.Where(c => !c.IsDelete).ToListAsync();
            ViewBag.Sizes = await _juanDbContext.Sizes.Where(s => !s.IsDelete).ToListAsync();

            if (!ModelState.IsValid) return View(product);
            if (!await _juanDbContext.Categories.Where(c => !c.IsDelete).AnyAsync(c => c.Id == product.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "categoryId is not correct...");
                return View(product);
            }
            foreach (int colorId in product.ColorIds)
            {
                if (!await _juanDbContext.Colors.AnyAsync(c => !c.IsDelete && c.Id == colorId))
                {
                    ModelState.AddModelError(nameof(Product.ColorIds), "Invalid color id");
                    return View(product);
                }
            }
            foreach (int sizeId in product.SizeIds)
            {
                if (!await _juanDbContext.Sizes.AnyAsync(s => !s.IsDelete && s.Id == sizeId))
                {
                    ModelState.AddModelError(nameof(Product.SizeIds), "Invalid size id");
                    return View(product);
                }
            }
            var mainFile = product.MainPhoto;
            var files = product.Photos;

            if (mainFile != null)
            {
                if (!mainFile.IsImage())
                {
                    ModelState.AddModelError(nameof(Product.MainPhoto), "Invalid format");
                    return View(product);
                }
                if (mainFile.IsCorrectSize(100))
                {
                    ModelState.AddModelError(nameof(Product.MainPhoto), "File limit exceeded");
                    return View(product);
                }
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","assets" ,"img", "product", existProduct.MainImage);
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);

                existProduct.MainImage = await mainFile.SaveFile("product");
            }

            if (files != null)
            {

                List<ProductImage> images = new();
                foreach (IFormFile file in files)
                {
                    if (!file.IsImage())
                    {
                        ModelState.AddModelError(nameof(Product.Photos), "Invalid format");
                        return View(product);
                    }
                    if (file.IsCorrectSize(100))
                    {
                        ModelState.AddModelError(nameof(Product.Photos), "File limit exceeded");
                        return View(product);
                    }
                    string filename = await file.SaveFile("product");
                    ProductImage image = new() { CreateDate = DateTime.Now, Name = filename, ProductId = existProduct.Id };
                    images.Add(image);
                }
                existProduct.ProductImages = images;
            }

            foreach (var productColor in existProduct.ProductColors)
            {
                productColor.IsDelete = true;
                productColor.DeleteDate = DateTime.Now;
            }
            List<ProductColor> colors = existProduct.ProductColors;
            foreach (var colorId in product.ColorIds)
            {
                ProductColor color = new() { CreateDate = DateTime.Now, ColorId = colorId, ProductId = existProduct.Id };
                colors.Add(color);
            }

            foreach (var productSize in existProduct.ProductSizes)
            {
                productSize.IsDelete = true;
                productSize.DeleteDate = DateTime.Now;
            }
            List<ProductSize> sizes = existProduct.ProductSizes;
            foreach (var sizeId in product.SizeIds)
            {
                ProductSize size = new() { CreateDate = DateTime.Now, Sizeid = sizeId, ProductId = existProduct.Id };
                sizes.Add(size);
            }


            existProduct.Name = product.Name;
            existProduct.Description = product.Description;
            existProduct.CategoryId = product.CategoryId;
            existProduct.Price = product.Price;
            existProduct.ExTax = product.ExTax;
            existProduct.DiscountPrice = product.DiscountPrice;
            existProduct.Count = product.Count;
            existProduct.SizeIds = product.SizeIds;
            existProduct.ColorIds = product.ColorIds;

            existProduct.UpdateDate=DateTime.Now;

            await _juanDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
