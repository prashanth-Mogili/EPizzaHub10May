using ePizzaHub.Core.Entities;
using ePizzaHub.Models;

namespace ePizzaHub.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        UserModel ValidateUser(string email, string password);
        bool CreateUser(User user, string Role);
    }
}
