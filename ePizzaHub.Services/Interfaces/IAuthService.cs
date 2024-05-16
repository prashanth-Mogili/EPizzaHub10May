using ePizzaHub.Core.Entities;
using ePizzaHub.Models;

namespace ePizzaHub.Services.Interfaces
{
    public interface IAuthService
    {
        UserModel ValidateUser(string email,string password);
        bool CreateUser(User user, string Role);
    }
}
