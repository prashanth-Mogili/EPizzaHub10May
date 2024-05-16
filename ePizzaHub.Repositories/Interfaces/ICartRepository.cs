using ePizzaHub.Core.Entities;
using ePizzaHub.Models;

namespace ePizzaHub.Repositories.Interfaces
{
    public interface ICartRepository: IRepository<Cart>
    {
        Cart GetCart(Guid CartId);
        CartModel GetCartDetails(Guid CartId);
        int DeleteItem(Guid CartId,int ItemId);
        int UpdateQuantity(Guid CartId, int ItemId,int Quantity);
        int UpdateCart(Guid CartId, int UserId);
    }
}
