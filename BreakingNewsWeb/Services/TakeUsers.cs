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
                //    new User(){ Name="Admin" , Email="Admin1989@bk.ru", Password="1234", Role = Role.admin  },
                //    new User(){ Name="Oleg" , Email="oleg@bk.ru", Password="4321", Role = Role.user  }
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
