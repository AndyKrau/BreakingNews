using BreakingNewsWeb.Migrations.UsersData;
using BreakingNewsWeb.Models;
using Microsoft.AspNetCore.Mvc;

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
            var existUserName = db.Users.FirstOrDefault(x=> x.Name == user.Name);
            // надо разобараться тут! 
            try
            {
                var newUser = new User
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
