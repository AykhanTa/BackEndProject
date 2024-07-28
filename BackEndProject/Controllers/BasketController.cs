using BackEndProject.Data;
using BackEndProject.Models;
using BackEndProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BackEndProject.Controllers
{
    public class BasketController : Controller
    {
        private readonly JuanDbContext _context;

        public BasketController(JuanDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> AddBasket(int id)
        {
            if (id == null) BadRequest();
            var product=await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            string basket= HttpContext.Request.Cookies["basket"];
            List<BasketVM> baskets;
            if (string.IsNullOrWhiteSpace(basket))
            {
                baskets = new();
            }
            else
            {
                baskets=JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            }
            if (baskets.Exists(p=>p.Id==id))
            {
                var basketProduct = baskets.FirstOrDefault(p => p.Id == id);
                basketProduct.Count++;
            }
            else
            {
                baskets.Add(new BasketVM() {
                    Id=product.Id,
                    Name=product.Name, 
                    Price=product.DiscountPrice>0?product.DiscountPrice:product.Price, 
                    Image=product.MainImage,
                    ExTax=product.ExTax,
                    Count=1 });

            }
            HttpContext.Response.Cookies.Append("basket",JsonConvert.SerializeObject(baskets));
            return PartialView("_BasketPartial",baskets);  

        }
        public IActionResult GetBasket()
        {
            return View();
        }
        public IActionResult RemoveProduct(int? id)
        {
            if (id == null) return BadRequest();
            var result = HttpContext.Request.Cookies["basket"];
            List<BasketVM> baskets= JsonConvert.DeserializeObject<List<BasketVM>>(result);
            var product=baskets.FirstOrDefault(p=>p.Id==id);
            baskets.Remove(product);
            HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(baskets));
            return RedirectToAction("index","home");
        }
    }
}
