using BreakingNewsWeb.Models;
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

        public HomeController(NewsContext newsContext, UsersContext usersContext)
        {
            // получаем контекст newsDB
            newsDb = newsContext;

            // получаем контекст usersDB
            usersDb = usersContext;
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
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            return RedirectToAction("Index");
        }

    }
}
