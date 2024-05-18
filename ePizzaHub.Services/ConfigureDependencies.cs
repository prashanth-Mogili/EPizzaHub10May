using ePizzaHub.Core;
using ePizzaHub.Core.Entities;
using ePizzaHub.Repositories.Implementations;
using ePizzaHub.Repositories.Interfaces;
using ePizzaHub.Services.Implementations;
using ePizzaHub.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Services
{
    public class ConfigureDependencies
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            //Register all dependencies here
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            //Register repositories
            services.AddScoped<IRepository<Item>, Repository<Item>>();
            services.AddScoped<IRepository<CartItem>, Repository<CartItem>>();
            services.AddScoped<IRepository<PaymentDetail>, Repository<PaymentDetail>>();

            services.AddScoped<IUserRepository, UserRepository>();           

            services.AddScoped<ICartRepository, CartRepository>();

            //Register Services

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IPaymentService, PaymentService>();
             

        }
    }
}
