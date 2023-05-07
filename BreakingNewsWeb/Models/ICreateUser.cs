using Microsoft.EntityFrameworkCore;

namespace BreakingNewsWeb.Models
{
    public interface ICreateUser
    {
        public User CreateNewUser(User user);
    }
}
