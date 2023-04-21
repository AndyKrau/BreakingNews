using Microsoft.EntityFrameworkCore;

namespace BreakingNewsWeb.Models;

public class ApplicationContext : DbContext
{
    public DbSet<Article> Articles { get; set; }
    public ApplicationContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Нужно посмотреть как передавать данные в строку подключения не в открытом виде
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=newsDB;Username=postgres;Password=1234");
    }

}
