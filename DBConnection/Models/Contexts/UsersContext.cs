using Microsoft.EntityFrameworkCore;
using DBConnection.Models.Classes;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DBConnection.Models.Contexts
{
    public class UsersContext : IdentityDbContext<User>
    {
        public UsersContext(DbContextOptions<UsersContext> options): base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=db_users;Username=postgres;Password=1234");
        }

    }
}
