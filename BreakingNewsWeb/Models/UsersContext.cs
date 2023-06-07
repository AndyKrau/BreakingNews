using BreakingNewsWeb.Migrations.UsersData;
using Microsoft.EntityFrameworkCore;
using System;

namespace BreakingNewsWeb.Models;

public class UsersContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public UsersContext(DbContextOptions<UsersContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(user => new { user.Name }).IsUnique(true);
    }
}
