using BackEndProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEndProject.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> ChangeUserStatus(string id)
        {
            var user= await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            user.IsBlocked=!user.IsBlocked;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }
    }
}
