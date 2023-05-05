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
            var context = HttpContext;
            var user = HttpContext.User.Identity;

            ViewData["currentUserName"] = user?.Name;
            ViewData["currentUserRole"] = context.User.FindFirst(ClaimTypes.Role)?.Value;

            return View();
        }
    }
}
