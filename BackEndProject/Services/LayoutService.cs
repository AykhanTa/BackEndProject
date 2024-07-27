using BackEndProject.Data;
using BackEndProject.Interfaces;
using BackEndProject.ViewModels;
using Newtonsoft.Json;

namespace BackEndProject.Services
{
    public class LayoutService : ILayoutService
    {
        private readonly JuanDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public LayoutService(JuanDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public IEnumerable<BasketVM> GetBasket()
        {
            List<BasketVM> list =new();
            string basket= _contextAccessor.HttpContext.Request.Cookies["basket"];
            if (string.IsNullOrEmpty(basket) )
            {
                return list;
            }
            else
            {
                list=JsonConvert.DeserializeObject<List<BasketVM>>(basket);
                foreach (var basketProduct in list)
                {
                    var existProduct = _context.Products.FirstOrDefault(p => p.Id == basketProduct.Id);
                    basketProduct.Name= existProduct.Name;
                    basketProduct.Price= existProduct.DiscountPrice>0?existProduct.DiscountPrice:existProduct.Price;
                    basketProduct.Image = existProduct.MainImage;
                    basketProduct.ExTax= existProduct.ExTax;

                }
                return list;
            }
        } 

        public IDictionary<string, string> GetSettings() => _context.Settings
            .Where(s => !s.IsDelete)
            .ToDictionary(s => s.Key, s => s.Value);

    }
}
