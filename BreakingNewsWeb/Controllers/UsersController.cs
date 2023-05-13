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

            // определяем переменные для модального окна
            ViewData["currentUserName"] = context.User.FindFirst(ClaimTypes.Name)?.Value;
            ViewData["currentUserRole"] = context.User.FindFirst(ClaimTypes.Role)?.Value;
            ViewData["currentUserEmail"] = context.User.FindFirst(ClaimTypes.Email)?.Value;
            ViewData["currentUserPostalCode"] = context.User.FindFirst(ClaimTypes.PostalCode)?.Value;
            ViewData["currentUserCountry"] = context.User.FindFirst(ClaimTypes.Country)?.Value;
            ViewData["currentUserMobilePhone"] = context.User.FindFirst(ClaimTypes.MobilePhone)?.Value;

            return View();
        }

        // Users/PersonalArea
        // Modal window Edit. Safe changes of users data
        [HttpPost]
        public async Task<IActionResult> SaveChange(User user)
        {
            // находим userId
            var currentUserNameIdentifier = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUserId = Int32.Parse(currentUserNameIdentifier!);

            // находим пользователя в базе по userId
            var existUser = usersDb.Users.FirstOrDefault(u => u.UserId == currentUserId);

            if (existUser != null)
            {
                // обновляем данные
                existUser.Name = user.Name;
                existUser.Email = user.Email;
                existUser.Country = user.Country;
                existUser.PhoneNumber = user.PhoneNumber;
                existUser.PostalCode = user.PostalCode;

                // сохраняем изменения
                usersDb.SaveChanges();

                // обновляем куки для карточки
                var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, existUser.UserId.ToString()),
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
