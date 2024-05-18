using ePizzaHub.Models;
using ePizzaHub.Services.Interfaces;
using ePizzaHub.UI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace ePizzaHub.UI.Controllers
{
    public class AccountController : Controller
    {
        IAuthService _authService;
        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = _authService.ValidateUser(model.Email, model.Password);
                if (user != null)
                {
                    GenerateTicket(user);
                    if(!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl); 
                    }
                    else if (user.Roles.Contains("Admin"))
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    else if (user.Roles.Contains("User"))
                    {
                        return RedirectToAction("Index", "Home", new { area = "User" });
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Credentials");
                }
            }
            return View();
        }

        private void GenerateTicket(UserModel user)
        {
            string strData = JsonSerializer.Serialize(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.UserData, strData),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, String.Join(",", user.Roles))
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
            };
        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal, properties);
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult UnAuthorize()
        {
            return View();
        }
    }
}
