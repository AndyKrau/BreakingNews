using BreakingNewsWeb.Services;
using Microsoft.EntityFrameworkCore;

namespace BreakingNewsWeb.Models
{
    public interface ITakeArticles
    {
        public DbSet<Article> MakeListArticles();
    }
}
