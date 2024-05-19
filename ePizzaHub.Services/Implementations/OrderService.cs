
using ePizzaHub.Core.Entities;
using ePizzaHub.Models;
using ePizzaHub.Repositories.Interfaces;
using ePizzaHub.Services.Interfaces;

namespace ePizzaHub.Services.Implementations
{
    public class OrderService : Service<Order>, IOrderService
    {
        IOrderRepository orderRepository;
        public OrderService(IOrderRepository repository) : base(repository)
        {
            orderRepository = repository;
        }

        public IEnumerable<Order> GetUserOrders(int UserId)
        {
           return orderRepository.GetUserOrders(UserId);
        }

        public int PlaceOrder(int userId, string orderId, string paymentId, CartModel cart, AddressModel address)
        {
            Order order = new Order
            {
                PaymentId = paymentId,
                UserId = userId,
                CreatedDate = DateTime.Now,
                Id = orderId,
                Street = address.Street,
                Locality = address.Locality,
                City = address.City,
                ZipCode = address.ZipCode,
                PhoneNumber = address.PhoneNumber,

            };
            foreach(var item in cart.Items)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ItemId = item.ItemId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Total = item.Total
                });
            }
            orderRepository.Add(order);
            return orderRepository.SaveChanges();

        }
    }
}
