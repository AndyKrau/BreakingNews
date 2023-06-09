using Microsoft.EntityFrameworkCore;
using DBConnection.Models.Classes;

namespace BreakingNewsWeb.Models
{
    public interface ICreateUser
    {
        public User CreateNewUser(User user);
    }
}
