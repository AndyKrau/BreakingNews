using BreakingNewsWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public IActionResult Articles()
        {
            List<Article> reverseArticles = Enumerable.Reverse(_articles).ToList();
            return View(reverseArticles);
        }
    }
}
