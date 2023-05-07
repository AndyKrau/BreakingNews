using BreakingNewsWeb.Migrations.UsersData;
using BreakingNewsWeb.Models;

namespace BreakingNewsWeb.Services
{
    public class CreateUser : ICreateUser
    {
        public User CreateNewUser(User user)
        {
            UsersContext db = new UsersContext();

            var newUser = new User
            {
                Name = user.Name,
                Password = user.Password,
                Email = user.Email,
                Country = user.Country,
                PhoneNumber = user.PhoneNumber,
                PostalCode = user.PostalCode,
            };

            db.Add(newUser);
            db.SaveChanges();

            return newUser;
        }
    }
}
