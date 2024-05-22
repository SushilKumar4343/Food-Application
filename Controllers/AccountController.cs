using FoodApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel log, string? returnurl)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(log.Email, log.Password, false, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnurl))
                        return LocalRedirect(returnurl);
                    return RedirectToAction("Index", "Home");
                }                   
                ModelState.AddModelError("", "Invalid Login Attempt");
               
            }
            return View(log);
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            //Response.Cookies.Delete(".AspNetCore.Identity.Application");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel reg)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    Name = reg.Name,
                    Address = reg.Address,
                    Email = reg.Email,
                    UserName = reg.Email
                };
                var result = await _userManager.CreateAsync(user,reg.Password);
                if (result.Succeeded)
                {
                    await _signInManager.PasswordSignInAsync(user, reg.Password, false, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                }
            }
            return View(reg);
        }

    }
}
