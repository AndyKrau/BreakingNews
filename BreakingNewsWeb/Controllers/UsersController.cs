using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BreakingNewsWeb.Models;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using DBConnection.Models.Contexts;
using DBConnection.Models.Classes;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace BreakingNewsWeb.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UsersContext usersDb;
        private readonly DbSet<User>? _users;

        public UsersController(UsersContext usersContext)
        {
            // получаем список пользователей 
            _users = usersContext.users;

            usersDb = usersContext;
        }

        // Users/Users

        [Authorize(Policy = "OnlyForAdmin")]
        public IActionResult Users()
        {
            var usersListWithRoleName = usersDb.users.Join(usersDb.roles,
                                        u => u.RoleId,
                                        r => r.RoleId,
                                        (u, r) => new
                                        {
                                            UserId = u.UserId,
                                            Name = u.Name,
                                            Email = u.Email,
                                            PhoneNumber = u.PhoneNumber,
                                            RoleId = u.RoleId,
                                            RoleName = r.RoleName,
                                            Country = u.Country,
                                            PostalCode = u.PostalCode,
                                        });
            ViewBag.Users = usersListWithRoleName;

            return View(usersListWithRoleName);
        }

        // Users/PersonalArea
        public IActionResult PersonalArea()
        {
            ViewBag.AdminRole = usersDb.roles.FirstOrDefault(x=> x.RoleName == "admin")?.RoleName.ToString();

            // получаем данные пользователя из куки для карточки пользователя
            var context = HttpContext;

            // определяем переменные для модального окна
            ViewData["currentUserName"] = context.User.FindFirst(ClaimTypes.Name)?.Value;
            ViewData["currentUserRole"] = context.User.FindFirst(ClaimTypes.Role)?.Value;

            //var roleName = context.User.FindFirst(ClaimTypes.Role)?.Value;
            //ViewData["currentUserRoleName"] = usersDb.roles.FirstOrDefault(x => x.RoleId.ToString() == roleName)?.RoleName.ToString(); 

            ViewData["currentUserEmail"] = context.User.FindFirst(ClaimTypes.Email)?.Value;
            ViewData["currentUserPostalCode"] = context.User.FindFirst(ClaimTypes.PostalCode)?.Value;
            ViewData["currentUserCountry"] = context.User.FindFirst(ClaimTypes.Country)?.Value;
            ViewData["currentUserMobilePhone"] = context.User.FindFirst(ClaimTypes.MobilePhone)?.Value;

            return View();
        }

        // Users/PersonalArea
        // Modal window Edit.Сохраняет данные после изменения карточки
        [HttpPost]
        public async Task<IActionResult> SaveChange(User user)
        {
            // находим userId
            var currentUserNameIdentifier = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUserId = Int32.Parse(currentUserNameIdentifier);

            // находим пользователя в базе по userId
            var existUser = usersDb.users.FirstOrDefault(u => u.UserId == currentUserId);

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
                        new Claim(ClaimTypes.Role, existUser.RoleId.ToString()),
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
