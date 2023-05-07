using BreakingNewsWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BreakingNewsWeb.Services
{
    public class TakeUsers : ITakeUsers
    {
        public DbSet<User> MakeListUsers()
        {
            // создаём новое подключение к БД
            UsersContext db = new UsersContext();

            #region Создание 2 пользователей для тестов
            //List<User> newUsers = new List<User>
            //{
            //    //new User { Name = "admin", Password = "1234", Email = "admin@admin.com", Role = Role.admin, Country = "USA", PhoneNumber = "+9444555332", PostalCode = "031231231" },
            //    new User { Name = "Oleg", Password = "4321", Email = "oleg@oleg.com", Role = Role.user, Country = "Russia", PhoneNumber = "+9334122321", PostalCode = "232423423" }
            //};

            //foreach (var item in newUsers)
            //{
            //    db.Users.AddRange(item);
            //}
            //db.SaveChanges();
            #endregion

            //получаем список статей из БД в переменную
            DbSet<User> users = db.Users;

            return users;
        }

       
    }
}
