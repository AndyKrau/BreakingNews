using Microsoft.EntityFrameworkCore;

namespace BreakingNewsWeb.Models;

public class NewsContext : DbContext
{
    public DbSet<Article> Articles { get; set; }
    public NewsContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Нужно посмотреть как передавать данные в строку подключения не в открытом виде
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=newsDB;Username=postgres;Password=1234");
    }

}
