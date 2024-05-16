using ePizzaHub.Core;
using ePizzaHub.Core.Entities;
using ePizzaHub.Models;
using ePizzaHub.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ePizzaHub.Repositories.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public bool CreateUser(User user, string Role)
        {
            try
            {
                Role role = _context.Roles.Where(r => r.Name == Role).FirstOrDefault();
                if(role != null)
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    user.Roles.Add(role);
                    _context.Users.Add(user);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public UserModel ValidateUser(string email, string password)
        {
            User user = _context.Users.Include(u => u.Roles)
                                      .Where(u => u.Email == email)
                                      .FirstOrDefault();
            if (user != null)
            {
                bool isValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
                if(isValid)
                {
                    UserModel userModel = new UserModel
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Roles = user.Roles.Select(r => r.Name).ToArray()
                    };
                    return userModel;
                }
            }
            return null;
        }
    }
}
