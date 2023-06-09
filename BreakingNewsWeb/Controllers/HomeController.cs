﻿using BreakingNewsWeb.Models;
using BreakingNewsWeb.Models.TestQueryToDb;
using BreakingNewsWeb.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DBConnection.Models.Contexts;
using DBConnection.Models.Classes;

namespace BreakingNewsWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly NewsContext newsDb;
        private readonly UsersContext usersDb;
        private readonly EducationContext educationDb;

        public HomeController(NewsContext newsContext, UsersContext usersContext, EducationContext educationContext)
        {
            // получаем контекст newsDB
            newsDb = newsContext;

            // получаем контекст usersDB
            usersDb = usersContext;

            // получаем контекст образовательной edudb
            educationDb = educationContext;

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

        public async Task<IActionResult> Index()
        {
            // количество выводимых статей на Index
            int articlesQuantity = 20;

            // Выбираем N последних статей из базы для Index, для вывода последних добавленных
            var reverseArticlesList = await newsDb.articles.OrderByDescending(a => a.Id).Take(articlesQuantity).ToListAsync();

            // передаём список статей в предствление
            return View(reverseArticlesList);
        }

        public IActionResult Articles(int page = 1)
        {
            // обозначаем количество статей на странице
            int articlesOnPage = 10;

            // создаём список статей с информацией о странице
            var data = new ArticlesListViewModel
            {
                Articles = newsDb.articles
                            .OrderByDescending(x => x.Id)
                            .Skip((page - 1) * articlesOnPage)
                            .Take(articlesOnPage)
                            .ToList(),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = articlesOnPage,
                    TotalItems = newsDb.articles.Count()
                }
            };
            return View(data);
        }

        public IActionResult About()
        {
            var result = educationDb.orders.Include(p => p.Product);

            var resultLinq = from o in educationDb.orders
                             join p in educationDb.products
                             on o.ProductId equals p.Id into ordersProductsGroup
                             from subProducts in ordersProductsGroup.DefaultIfEmpty()
                             select new AllQueries
                             {
                                 orderNumder = o.Id,
                                 orderTime = o.CreatedAt,
                                 orderPrice = o.Price,
                                 productName = subProducts.ProductName,
                                 productCompany = subProducts.Company,
                                 productPrice = subProducts.Price,
                             };

            var resultLinq2 = educationDb.orders.Join(educationDb.products,
                                                    o => o.ProductId,
                                                    p => p.Id,
                                                    (o, p) => new
                                                    {
                                                        orderNumder = o.Id,
                                                        orderTime = o.CreatedAt,
                                                        orderPrice = o.Price,
                                                        productName = p.ProductName,
                                                        productCompany = p.Company,
                                                        productPrice = p.Price,
                                                    });

            ViewBag.OrderByNav = result;
            ViewBag.OrdersByLINQ = resultLinq;
            ViewBag.OrdersByLINQ2 = resultLinq2;

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string ReturnUrl)
        {
            // Поиск пользователя по имени в базе
            var existUser = usersDb.users.FirstOrDefault(x => x.Name == username);

            // Если пользователь найден проверяем пароль
            if (existUser != null)
            {
                bool isValidPass = BCrypt.Net.BCrypt.Verify(password, existUser?.Password);

                if (isValidPass)
                {
                    var userRoleName = usersDb.roles.FirstOrDefault(x => x.RoleId.ToString() == existUser.RoleId.ToString())?.RoleName.ToString();
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, existUser.UserId.ToString()),
                        new Claim(ClaimTypes.Name, username),
                        new Claim(ClaimTypes.Role, userRoleName),
                        new Claim(ClaimTypes.Email, existUser.Email),
                        new Claim(ClaimTypes.MobilePhone, existUser.PhoneNumber??""),
                        new Claim(ClaimTypes.PostalCode, existUser.PostalCode??""),
                        new Claim(ClaimTypes.Country, existUser.Country??""),
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return Redirect(ReturnUrl ?? "/Users/PersonalArea");
                }

                else return View();
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

            if (_newUser != null)
            {
                // помещаем данные о пользователе в cookie и редиректим в ЛК
                var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, _newUser.UserId.ToString()),
                        new Claim(ClaimTypes.Name, _newUser.Name),
                        new Claim(ClaimTypes.Role, _newUser.RoleId.ToString()),
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
                var existUser = usersDb.users.FirstOrDefault(a => a.UserId == id);

                if (existUser != null)
                {
                    usersDb.users.Entry(existUser).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
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
