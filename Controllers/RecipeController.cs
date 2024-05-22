using FoodApplication.ContextDBCongi;
using FoodApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodApplication.Controllers
{
    public class RecipeController : Controller
    {
        //complete but problem occur RecipeId notcome
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly FoodDbContext _context;

        public RecipeController(UserManager<ApplicationUser> userManager, FoodDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetRecipeCard([FromBody] List<Recipe> recipes)
        {
            return PartialView("_RecipeCard",recipes);
        }
        public IActionResult Search([FromQuery] string recipe)
        {
            ViewBag.Recipe = recipe;
            return View();
        }
        public IActionResult Order([FromQuery] string id)
        {
            ViewBag.Id = id;
            return View();
        }
        [HttpPost]
        [Authorize]
        public IActionResult Order([FromForm]Order order)
        {
            order.OrderDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Orders.Add(order);
                _context.SaveChanges();
                return RedirectToAction("Index", "Recipe");
            }
            return RedirectToAction("Index", "Recipe", new {id=order.Id}); ;
        }
        [HttpPost]
        public async Task<IActionResult> ShowOrder(OrderRecipeDetails orderRecipeDetails)
        {
            Random random = new Random();
            ViewBag.Price = Math.Round(random.Next(150, 500)/5.0)*5;

            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.UserId = user?.Id;
            ViewBag.Address = user?.Address;
            return PartialView("_ShowOrder", orderRecipeDetails);
        }
    }
}
