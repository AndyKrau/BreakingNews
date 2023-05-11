using BreakingNewsWeb.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BreakingNewsWeb.Controllers
{
    public class HomeController : Controller
    {
        public NewsContext newsDb;
        public UsersContext usersDb;

        public HomeController(NewsContext newsContext, UsersContext usersContext)
        {
            // получаем контекст newsDB
            newsDb = newsContext;

            // получаем контекст usersDB
            usersDb = usersContext;

        }

        public IActionResult Index()
        {
            // Переворачиваем список статей из БД, я вывода последних добавленных
            List<Article> reverseArticles = Enumerable.Reverse(newsDb.Articles).ToList();

            // передаём список статей в предствление
            return View(reverseArticles);
        }

        public IActionResult Articles(int page = 0)
        {
            List<Article> reverseArticles = Enumerable.Reverse(newsDb.Articles).ToList();
            int pageSize = 5;
            int count = reverseArticles.Count();

            var data = reverseArticles.Skip(page * pageSize).Take(pageSize).ToList();
            ViewBag.MaxPage = (count / pageSize) - (count % pageSize == 0 ? 1 : 0);
            ViewBag.Page = page;

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
            List<User> usersFromDb = usersDb.Users!.ToList();

            foreach (User user in usersFromDb)
            {
                if (username == user.Name && password == user.Password)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, username),
                        new Claim(ClaimTypes.Role, user.Role.ToString()),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.MobilePhone, user.PhoneNumber??""),
                        new Claim(ClaimTypes.PostalCode, user.PostalCode??""),
                        new Claim(ClaimTypes.Country, user.Country??""),
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
        public async Task<IActionResult> CreateUser(User user, [FromServices] ICreateUser createUser)
        {

            // создаём пользователя и помещаем в базу
            var _newUser = createUser.CreateNewUser(user);

            // помещаем данные о пользователе в cookie и редиректим в ЛК
            var claims = new List<Claim>()
                    {
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
