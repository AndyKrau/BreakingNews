using BreakingNewsWeb.Models;
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

            // пробую получать имя на всех старницах 

            var context = HttpContext;

            var _user = context.User.Identity;

            ViewData["currentUserName"] = _user?.Name;

            // пробую получать имя на всех старницах 

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


            // пробую получать имя на всех старницах 

            var currentUser = HttpContext.User.Identity;
            ViewData["currentUserName"] = currentUser?.Name;

            // пробую получать имя на всех старницах 


            return View(data);
        }

        public IActionResult About()
        {
            // пробую получать имя на всех старницах 

            var currentUser = HttpContext.User.Identity;
            ViewData["currentUserName"] = currentUser?.Name;

            // пробую получать имя на всех старницах 

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string ReturnUrl)
        {
            List<User> usersFromDb = _users.ToList();

            foreach (User user in usersFromDb)
            {
                if (username == user.Name && password == user.Password)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, username),
                        new Claim(ClaimTypes.Role, user.Role.ToString()),
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


    }
}
