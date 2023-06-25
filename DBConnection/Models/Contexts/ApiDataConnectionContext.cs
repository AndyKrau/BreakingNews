using DBConnection.Models.Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConnection.Models.Contexts
{
    public class ApiDataConnectionContext :DbContext
    {
        public DbSet<ApiData> ApiData { get; set; }
        public DbSet<Country> Countries { get; set; }
        public ApiDataConnectionContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=db_api_data;Username=postgres;Password=1234");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>().HasData(
                new Country { Id = 1, CountryName = "USA", CountryKey = "us" },
                new Country { Id = 2, CountryName = "Russia", CountryKey = "ru" },
                new Country { Id = 3, CountryName = "China", CountryKey = "ch" },
                new Country { Id = 4, CountryName = "France", CountryKey = "fr" }
                );

            modelBuilder.Entity<ApiData>().HasData(
                new ApiData { Id = 1, ApiKey = "", Url = "https://newsapi.org/v2/top-headlines?", CountryId = 1}
                );
        }
    }
}
