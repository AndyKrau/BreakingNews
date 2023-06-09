using Microsoft.EntityFrameworkCore;
using DBConnection.Models.Classes;
using BCrypt.Net;
using System.Security.Cryptography.X509Certificates;

namespace DBConnection.Models.Contexts
{
    public class UsersContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<Role> roles { get; set; }

        public UsersContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=db_users;Username=postgres;Password=1234");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Users table set settings:
                // поля UserId, Name, Email индексируем и делаем уникальными
                modelBuilder.Entity<User>().HasIndex(p => new { p.UserId, p.Name, p.Email}).IsUnique(true);

                // формируем нужный регистр имени таблицы user
                modelBuilder.Entity<User>().ToTable("users", schema: "public");

                // формируем нужный регистр имени столбцов таблицы users
                modelBuilder.Entity<User>().Property(p => p.UserId).HasColumnName("user_id");
                modelBuilder.Entity<User>().Property(p => p.Name).HasColumnName("name");
                modelBuilder.Entity<User>().Property(p => p.Password).HasColumnName("password");
                modelBuilder.Entity<User>().Property(p => p.Email).HasColumnName("email");
                modelBuilder.Entity<User>().Property(p => p.PhoneNumber).HasColumnName("phone_number");
                modelBuilder.Entity<User>().Property(p => p.PostalCode).HasColumnName("postal_code");
                modelBuilder.Entity<User>().Property(p => p.Country).HasColumnName("country");
                modelBuilder.Entity<User>().Property(p => p.RoleId).HasColumnName("role_id");
                modelBuilder.Entity<User>().Property(p => p.CreatedAt).HasColumnName("created_at");

            #endregion

            #region Roles table set settings:
                // поля RoleId индексируем и делаем уникальными
                modelBuilder.Entity<Role>().HasIndex(p=> p.RoleId).IsUnique(true);

                // формируем нужный регистр имени таблицы roles
                modelBuilder.Entity<Role>().ToTable("roles", schema: "public");

                // формируем нужный регистр имени столбцов таблицы roles
                modelBuilder.Entity<Role>().Property(p => p.RoleId).HasColumnName("role_id");
                modelBuilder.Entity<Role>().Property(p => p.RoleName).HasColumnName("role_name");
            #endregion


            #region Формируем начальные данные таблиц Role и User
            modelBuilder.Entity<Role>().HasData(
                    new Role[]
                    {
                        new Role { RoleId = 1, RoleName= "admin"},
                        new Role { RoleId = 2, RoleName= "user"}
                    });

                modelBuilder.Entity<User>().HasData(
                    new User
                    {
                        UserId = 1,
                        Name = "admin",
                        Password = BCrypt.Net.BCrypt.HashPassword("1234"),
                        Email = "admin@admin.com",
                        RoleId = 1
                    });
            #endregion


        }
    }
}
