using ePizzaHub.Core.Entities;

namespace ePizzaHub.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        IEnumerable<Order> GetUserOrders(int UserId);

    }
}
