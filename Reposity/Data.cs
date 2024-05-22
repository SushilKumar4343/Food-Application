using FoodApplication.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FoodApplication.Reposity
{
    public class Data : IData
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public Data(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GetUser(ClaimsPrincipal claims)
        {
            return await _userManager.GetUserAsync(claims);
        }
    }
}
