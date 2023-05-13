using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BreakingNewsWeb.Models;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace BreakingNewsWeb.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        public UsersContext usersDb;
        public DbSet<User>? _users;
        public UsersController(UsersContext usersContext)
        {
            // получаем список пользователей 
            _users = usersContext.Users;

            usersDb = usersContext;
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

        [HttpPost]
        public async Task<IActionResult> SaveChange(User user)
        {
            var context = HttpContext;

            var currentUserId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var x = Int32.Parse(currentUserId!);

            var existUser = usersDb.Users.FirstOrDefault(u => u.UserId == x);

            if (existUser != null)
            {
                existUser.Name = user.Name;
                existUser.Email = user.Email;
                existUser.Country = user.Country;
                existUser.PhoneNumber = user.PhoneNumber;
                existUser.PostalCode = user.PostalCode;

                usersDb.SaveChanges();

                var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, existUser.Name),
                        new Claim(ClaimTypes.Role, existUser.Role.ToString()),
                        new Claim(ClaimTypes.Email, existUser.Email),
                        new Claim(ClaimTypes.MobilePhone, existUser.PhoneNumber??""),
                        new Claim(ClaimTypes.PostalCode, existUser.PostalCode??""),
                        new Claim(ClaimTypes.Country, existUser.Country??""),
                    };

                var claimsIdentity = new ClaimsIdentity(claims, "SaveChange");

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                
                return Redirect("/Users/PersonalArea");
            }
        
            return Redirect("/Users/PersonalArea");
        }
    }
}
