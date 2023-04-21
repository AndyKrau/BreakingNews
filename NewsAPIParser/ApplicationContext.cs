using Microsoft.EntityFrameworkCore;


namespace NewsAPIParser
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public ApplicationContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=newsDB;Username=postgres;Password=1234");
        }
    }
}
