using FoodApplication.Models;
using System.Security.Claims;

namespace FoodApplication.Reposity
{
    public interface IData
    {
        Task<ApplicationUser> GetUser(ClaimsPrincipal claims);
    }
}
