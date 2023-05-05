using Microsoft.EntityFrameworkCore;

namespace BreakingNewsWeb.Models;

public class UsersContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public UsersContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Нужно посмотреть как передавать данные в строку подключения не в открытом виде
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=usersDB;Username=postgres;Password=1234");
    }

}
