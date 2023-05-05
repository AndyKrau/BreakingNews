using BreakingNewsWeb.Services;
using Microsoft.EntityFrameworkCore;

namespace BreakingNewsWeb.Models
{
    public interface ITakeUsers
    {
        public DbSet<User> MakeListUsers();
    }
}
