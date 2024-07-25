using BackEndProject.Interfaces;
using BackEndProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackEndProject.Controllers
{
    public class ContactController : Controller
    {
        private readonly ILayoutService _layoutService;

        public ContactController(ILayoutService layoutService)
        {
            _layoutService = layoutService;
        }

        public IActionResult Index()
        {
            return View(_layoutService.GetSettings());
        }
    }
}
