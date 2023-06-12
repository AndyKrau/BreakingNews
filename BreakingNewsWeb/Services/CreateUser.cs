using BreakingNewsWeb.Models;
using Microsoft.AspNetCore.Mvc;
using DBConnection.Models.Contexts;
using DBConnection.Models.Classes;

namespace BreakingNewsWeb.Services
{
    public class CreateUser : ICreateUser
    {
        readonly UsersContext db;
        public CreateUser(UsersContext usersContext)
        {
            db = usersContext;
        }
        public User CreateNewUser(User user)
        {
            User newUser = user;
            var existUserName = db.users.FirstOrDefault(x=> x.Name == user.Name);
            var existUserEmail = db.users.FirstOrDefault(e=> e.Email == user.Email);

            if (existUserName == null)
            {
                if (existUserEmail == null)
                {
                    newUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    db.users.Add(newUser);
                    db.SaveChanges();
                    
                    return newUser;
                }
            }
            return null;

            // надо разобараться тут! 
            //try
            //{
            //    User newUser = new()
            //    {
            //        Name = user.Name,
            //        Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
            //        Email = user.Email,
            //        Country = user.Country,
            //        PhoneNumber = user.PhoneNumber,
            //        PostalCode = user.PostalCode,
            //    };

            //    db.Add(newUser);
            //    db.SaveChanges();

            //    return newUser;
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error: {ex.Message}");
            //}



        }
    }
}
