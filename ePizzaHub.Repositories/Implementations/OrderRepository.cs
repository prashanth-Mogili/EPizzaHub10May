using ePizzaHub.Core;
using ePizzaHub.Core.Entities;
using ePizzaHub.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ePizzaHub.Repositories.Implementations
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext db) : base(db)
        {
            
        }
        public IEnumerable<Order> GetUserOrders(int UserId)
        {
            return _context.Orders.Include(o => o.OrderItems)
                .Where(o => o.UserId == UserId).ToList();
        }
    }
}
