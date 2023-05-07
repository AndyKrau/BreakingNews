using BreakingNewsWeb.Models;
using BreakingNewsWeb.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using ReflectionIT.Mvc.Paging;
using System.Numerics;
//using PagedList.Core;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BreakingNewsWeb.Controllers
{
    public class HomeController : Controller
    {
        public DbSet<Article> _articles;
        public DbSet<User>? _users;

        public HomeController(ITakeArticles articles, ITakeUsers users)
        {
            // получаем список статей при обращении к контроллеру
            _articles = articles.MakeListArticles();

            // получаем список пользователей из БД usersDB
            _users = users.MakeListUsers();
           }

        public IActionResult Index()
        {
            // Переворачиваем список статей из БД, я вывода последних добавленных
            List<Article> reverseArticles = Enumerable.Reverse(_articles).ToList();

            // получаем имя пользователя из куки
            var _user = HttpContext.User.Identity;
            ViewData["currentUserName"] = _user?.Name;

            // передаём список статей в предствление
            return View(reverseArticles);
        }

        public IActionResult Articles(int page = 0)
        {
            List<Article> reverseArticles = Enumerable.Reverse(_articles).ToList();
            int pageSize = 5;
            int count = reverseArticles.Count();

            var data = reverseArticles.Skip(page * pageSize).Take(pageSize).ToList();
            ViewBag.MaxPage = (count / pageSize) - (count % pageSize == 0 ? 1 : 0);
            ViewBag.Page = page;


            // получаем имя пользователя из куки
            var currentUser = HttpContext.User.Identity;
            ViewData["currentUserName"] = currentUser?.Name;

            return View(data);
        }

        public IActionResult About()
        {
            // получаем имя пользователя из куки
            var currentUser = HttpContext.User.Identity;
            ViewData["currentUserName"] = currentUser?.Name;

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string ReturnUrl)
        {
            List<User> usersFromDb = _users!.ToList();

            foreach (User user in usersFromDb)
            {
                if (username == user.Name && password == user.Password)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, username),
                        new Claim(ClaimTypes.Role, user.Role.ToString()),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                        new Claim(ClaimTypes.PostalCode, user.PostalCode),
                        new Claim(ClaimTypes.Country, user.Country),
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return Redirect(ReturnUrl == null ? "/Users/PersonalArea" : ReturnUrl);
                }
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
        public async Task<IActionResult> CreateUser(User user, CreateUser creater)
        {
            // создаём пользователя и помещаем в базу
            var _newUser = creater.CreateNewUser(user);

            // помещаем данные о пользователе в cookie и редиректив в ЛК
            var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, _newUser.Name),
                        new Claim(ClaimTypes.Role, _newUser.Role.ToString()),
                        new Claim(ClaimTypes.Email, _newUser.Email),
                        new Claim(ClaimTypes.MobilePhone, _newUser.PhoneNumber),
                        new Claim(ClaimTypes.PostalCode, _newUser.PostalCode),
                        new Claim(ClaimTypes.Country, _newUser.Country),
                    };

            var claimsIdentity = new ClaimsIdentity(claims, "CreateUser");

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return Redirect("/Users/PersonalArea");

           
        }

    }
}
