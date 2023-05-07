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
        public UsersController(ITakeUsers users)
        {
            // получаем список пользователей 
            _users = users.MakeListUsers();
        }
        public IActionResult Users()
        {
            return View(_users);
        }
        public IActionResult PersonalArea()
        {
            // получаем имя пользователя и роль из куки
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
