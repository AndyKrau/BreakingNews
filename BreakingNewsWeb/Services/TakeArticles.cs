using BreakingNewsWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BreakingNewsWeb.Services
{
    public class TakeArticles : ITakeArticles
    {
        public DbSet<Article> MakeListArticles()
        {
            // создаём новое подключение к БД
            NewsContext db = new NewsContext();

            //получаем список статей из БД в переменную
            DbSet<Article> articles = db.Articles;

            return articles;
        }
    }
}
