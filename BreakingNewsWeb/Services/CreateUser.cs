using BreakingNewsWeb.Models;
using Microsoft.AspNetCore.Mvc;
using DBConnection.Models.Contexts;
using DBConnection.Models.Classes;

namespace BreakingNewsWeb.Services
{
    public class CreateUser : ICreateUser
    {
        UsersContext db;
        public CreateUser(UsersContext usersContext)
        {
            db = usersContext;
        }
        public User CreateNewUser(User user)
        {
            var existUserName = db.users.FirstOrDefault(x=> x.Name == user.Name);
            // надо разобараться тут! 
            try
            {
                User newUser = new()
                {
                    Name = user.Name,
                    Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
                    Email = user.Email,
                    Country = user.Country,
                    PhoneNumber = user.PhoneNumber,
                    PostalCode = user.PostalCode,
                };

                db.Add(newUser);
                db.SaveChanges();

                return newUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return null;

        }
    }
}
