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
            //получаем список статей при обращении к контроллеру
            _articles = articles.MakeListArticles();
        }

        public IActionResult Index()
        {
            return View(_articles); //передаём список статей в предствление
        }
    }
}
