using Microsoft.EntityFrameworkCore;

namespace BreakingNewsWeb.Models;

public class _NewsContext : DbContext
{
    public DbSet<_Article> Articles { get; set; }
    public _NewsContext(DbContextOptions<_NewsContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}
