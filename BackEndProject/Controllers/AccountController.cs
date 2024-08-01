using BackEndProject.Helpers;
using BackEndProject.Models;
using BackEndProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace BackEndProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);
            AppUser user = new()
            {
                UserName = registerVM.UserName,
                Email = registerVM.Email,
                FullName= registerVM.FullName,
            };
            IdentityResult result= await _userManager.CreateAsync(user,registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
                return View(registerVM);
            }
                
            await _userManager.AddToRoleAsync(user,UserRoles.member.ToString());

            return RedirectToAction("Index","Home");

        }



        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM,string returnUrl)
        {
            if (!ModelState.IsValid) return View(loginVM);
            AppUser user=await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail);
            if (user is null)
            {
                user = await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);
                if (user is null)
                {
                    ModelState.AddModelError("", "user not found...");
                    return View(loginVM);
                }
            }
            SignInResult result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "account isLocked out...");
                return View(loginVM);
            }
            
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "username or password is wrong...");
                return View(loginVM);
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains(UserRoles.admin.ToString()))
                return RedirectToAction("Index", "dashboard", new {area="manage"});

            if (returnUrl is null)
                return RedirectToAction("Index","Home");
            return Redirect(returnUrl);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        //public async Task<IActionResult> Role()
        //{
        //    if (!await _roleManager.RoleExistsAsync("admin"))
        //        await _roleManager.CreateAsync(new() { Name="admin"});
        //    if (!await _roleManager.RoleExistsAsync("member"))
        //        await _roleManager.CreateAsync(new() { Name = "member" });
        //    if (!await _roleManager.RoleExistsAsync("superadmin"))
        //        await _roleManager.CreateAsync(new() { Name = "superadmin" });
        //    return Content("Roles added");
        //}

    }
}
