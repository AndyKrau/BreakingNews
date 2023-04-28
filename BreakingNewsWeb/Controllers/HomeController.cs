using BreakingNewsWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;

namespace BreakingNewsWeb.Controllers
{
    public class HomeController : Controller
    {
        public DbSet<Article> _articles;

        public HomeController(ITakeArticles articles)
        {
            // получаем список статей при обращении к контроллеру
            _articles = articles.MakeListArticles();
        }

        public IActionResult Index()
        {
            // Переворачиваем список статей из БД, я вывода последних добавленных
            List<Article> reverseArticles = Enumerable.Reverse(_articles).ToList();

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

            return View(data);



            // List<Article> reverseArticles = Enumerable.Reverse(_articles).ToList();
            //return View(reverseArticles);

        }
    }
}
