using BreakingNewsWeb.Migrations.UsersData;
using BreakingNewsWeb.Models;
using BreakingNewsWeb.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BreakingNewsWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly NewsContext newsDb;
        private readonly UsersContext usersDb;

        public HomeController(NewsContext newsContext, UsersContext usersContext)
        {
            // получаем контекст newsDB
            newsDb = newsContext;

            // получаем контекст usersDB
            usersDb = usersContext;

            #region Adding default admin
            // var newUser = new User
            // {
            //    Name = "admin",
            //    Password = BCrypt.Net.BCrypt.HashPassword("1234"),
            //    Email = "admin@admin.com",
            //    Country = "Russia",
            //    PhoneNumber = "+7890123123",
            //    PostalCode = "99999999",
            //    Role = Role.admin
            // };
            // usersDb.Add(newUser);
            // usersDb.SaveChanges();
            #endregion

        }

        public IActionResult Index()
        {
            // количество выводимых статей на Index
            int articlesQuantity = 20;

            // Выбираем N последних статей из базы для Index, для вывода последних добавленных
            var reverseArticlesList = newsDb.Articles.OrderByDescending(x => x.Id).Take(articlesQuantity).ToList();

            // передаём список статей в предствление
            return View(reverseArticlesList);
        }

        public IActionResult Articles(int page = 1)
        {
            // обозначаем количество статей на странице
            int articlesOnPage = 5;

            // создаём список статей с информацией о странице
            var data = new ArticlesListViewModel
            {
                Articles = newsDb.Articles
                            .OrderByDescending(x => x.Id)
                            .Skip((page - 1) * articlesOnPage)
                            .Take(articlesOnPage)
                            .ToList(),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = articlesOnPage,
                    TotalItems = newsDb.Articles.Count()
                }
            };        
            return View(data);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string ReturnUrl)
        {
            var existUser = usersDb.Users.FirstOrDefault(x => x.Name == username);

            bool isValidPass = BCrypt.Net.BCrypt.Verify(password, existUser?.Password);

                if (isValidPass)
                {
                    var claims = new List<Claim>()
                    { 
                        new Claim(ClaimTypes.NameIdentifier, existUser.UserId.ToString()),
                        new Claim(ClaimTypes.Name, username),
                        new Claim(ClaimTypes.Role, existUser.Role.ToString()),
                        new Claim(ClaimTypes.Email, existUser.Email),
                        new Claim(ClaimTypes.MobilePhone, existUser.PhoneNumber??""),
                        new Claim(ClaimTypes.PostalCode, existUser.PostalCode??""),
                        new Claim(ClaimTypes.Country, existUser.Country??""),
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return Redirect(ReturnUrl == null ? "/Users/PersonalArea" : ReturnUrl);
                }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user, [FromServices] ICreateUser createUser)
        {
            // создаём пользователя и помещаем в базу
            var _newUser = createUser.CreateNewUser(user);

            if(_newUser != null)
            {
                // помещаем данные о пользователе в cookie и редиректим в ЛК
                var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, _newUser.UserId.ToString()),
                        new Claim(ClaimTypes.Name, _newUser.Name),
                        new Claim(ClaimTypes.Role, _newUser.Role.ToString()),
                        new Claim(ClaimTypes.Email, _newUser.Email),
                        new Claim(ClaimTypes.MobilePhone, _newUser.PhoneNumber??""),
                        new Claim(ClaimTypes.PostalCode, _newUser.PostalCode??""),
                        new Claim(ClaimTypes.Country, _newUser.Country??""),
                    };

                var claimsIdentity = new ClaimsIdentity(claims, "CreateUser");

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return Redirect("/Users/PersonalArea");
            }

            return Redirect("/Users/PersonalArea");
        }

        public IActionResult DeleteUser(int id)
        {
            return View(id);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int? id)
        {
            if (id != null)
            {
                var existUser = usersDb.Users.FirstOrDefault(a => a.UserId == id);

                if (existUser != null)
                {
                    usersDb.Users.Entry(existUser).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    await usersDb.SaveChangesAsync();
                    return Redirect("/Users/Users");
                }
            }
            return Redirect("/Users/Users");
        }

        public IActionResult AccessDenied()
        {
            var context = HttpContext;
            context.Response.StatusCode = 403;
            ViewData["Error"] = context.Response.StatusCode;

            return View();
        }

    }
}
