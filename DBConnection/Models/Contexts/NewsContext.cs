using Microsoft.EntityFrameworkCore;
using DBConnection.Models.Classes;

namespace DBConnection.Models.Contexts
{
    public class NewsContext : DbContext
    {
        // нарушаем правила именования свойств для создание нужного имени таблицы, в дальнейшем удобнее будет пиcать запросы
        public DbSet<Article> articles { get; set; }
        public NewsContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=db_articles;Username=postgres;Password=1234");
        }
    }
}
