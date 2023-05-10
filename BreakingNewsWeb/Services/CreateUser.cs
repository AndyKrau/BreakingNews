using BreakingNewsWeb.Migrations.UsersData;
using BreakingNewsWeb.Models;

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
