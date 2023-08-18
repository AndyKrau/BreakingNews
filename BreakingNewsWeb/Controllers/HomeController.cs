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

        public HomeController(NewsContext newsContext)
        {
            // получаем контекст newsDB
            newsDb = newsContext;
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

        public async Task<IActionResult> Articles(string? source, int page = 1)
        {
            // обозначаем количество статей на странице
            int articlesOnPage = 7;

            // создаём список статей с информацией о странице
            var data = new ArticlesListViewModel
            {

                Articles = await newsDb.articles
                            .Where(p => source == null || p.Source == source)
                            .OrderByDescending(x => x.Id)
                            .Skip((page - 1) * articlesOnPage)
                            .Take(articlesOnPage)
                            .ToListAsync(),
                PagingInfo = new PagingInfo
                            {
                                CurrentPage = page,
                                ItemsPerPage = articlesOnPage,
                                TotalItems = source == null ? newsDb.articles.Count() : newsDb.articles.Where(a => a.Source == source).Count()
                            },
                CurrentSource = source
            };
            return View(data);
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
