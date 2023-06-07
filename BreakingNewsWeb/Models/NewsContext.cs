using Microsoft.EntityFrameworkCore;

namespace BreakingNewsWeb.Models;

public class NewsContext : DbContext
{
    public DbSet<Article> Articles { get; set; }
    public NewsContext(DbContextOptions<NewsContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}
