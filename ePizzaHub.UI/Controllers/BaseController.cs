using ePizzaHub.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace ePizzaHub.UI.Controllers
{
    public class BaseController : Controller
    {
       public UserModel CurrentUser
        {
            get
            {
                if(User.Claims.Count() > 0)
                {
                    string userData = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData).Value;
                    return JsonSerializer.Deserialize<UserModel>(userData);
                }
                return null;
            }
        }
    }
}
