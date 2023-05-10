using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BreakingNewsWeb.Models;
using System.Net;
using System.Security.Claims;

namespace BreakingNewsWeb.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        public DbSet<User>? _users;
        public UsersController(UsersContext usersContext)
        {
            // получаем список пользователей 
            //_users = users.MakeListUsers();
            _users = usersContext.Users;
        }

        // Users/Users

        [Authorize(Policy = "OnlyForAdmin")]
        public IActionResult Users()
        {
            return View(_users);
        }

        // Users/PersonalArea
        public IActionResult PersonalArea()
        {
            // получаем данные пользователя из куки для карточки пользователя
            var context = HttpContext;
            var user = HttpContext.User.Identity;

            ViewData["currentUserName"] = user?.Name;
            ViewData["currentUserRole"] = context.User.FindFirst(ClaimTypes.Role)?.Value;
            ViewData["currentUserEmail"] = context.User.FindFirst(ClaimTypes.Email)?.Value;
            ViewData["currentUserPostalCode"] = context.User.FindFirst(ClaimTypes.PostalCode)?.Value;
            ViewData["currentUserCountry"] = context.User.FindFirst(ClaimTypes.Country)?.Value;
            ViewData["currentUserMobilePhone"] = context.User.FindFirst(ClaimTypes.MobilePhone)?.Value;

            return View();
        }
    }
}
