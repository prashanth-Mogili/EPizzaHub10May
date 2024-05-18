using ePizzaHub.Core.Entities;
using ePizzaHub.Models;
using ePizzaHub.Repositories.Interfaces;
using ePizzaHub.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Runtime.CompilerServices;

namespace ePizzaHub.Services.Implementations
{
    public class CartService : Service<Cart>, ICartService
    {
        ICartRepository _cartRepo;
        IRepository<CartItem> _cartItemRepo;
        IConfiguration _config;

        public CartService(ICartRepository cartRepository, IRepository<CartItem> cartItem,IConfiguration config) : base(cartRepository)
        {
            _cartRepo = cartRepository;
            _cartItemRepo = cartItem;
            _config = config;
        }

        public Cart AddItem(int UserId, Guid CartId, int ItemId, decimal UnitPrice, int Quantity)
        {
            try
            {
                Cart cart = _cartRepo.GetCart(CartId);
                if (cart == null)
                {
                    cart = new Cart
                    {
                        UserId = UserId,
                        Id = CartId,
                        CreatedDate = DateTime.UtcNow,
                        IsActive = true
                    };
                    CartItem cartItem = new CartItem
                    {
                        CartId = CartId,
                        ItemId = ItemId,
                        Quantity = Quantity,
                        UnitPrice = UnitPrice
                    };
                    cart.CartItems.Add(cartItem);
                    _cartRepo.Add(cart);
                    _cartItemRepo.SaveChanges();
                }

                else
                {
                    CartItem cartItem = cart.CartItems.Where(x => x.ItemId == ItemId).FirstOrDefault();
                    if (cartItem != null)
                    {
                        cartItem.Quantity += Quantity;
                        _cartItemRepo.Update(cartItem);
                        _cartItemRepo.SaveChanges();
                    }
                    else
                    {
                        cartItem = new CartItem
                        {
                            CartId = CartId,
                            ItemId = ItemId,
                            Quantity = Quantity,
                            UnitPrice = UnitPrice
                        };
                        cart.CartItems.Add(cartItem);
                        _cartItemRepo.Update(cartItem);
                        _cartItemRepo.SaveChanges();
                    }
                }
                return cart;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int DeleteItem(Guid CartId, int ItemId)
        {
            return _cartRepo.DeleteItem(CartId,ItemId);
        }

        public int GetCartCount(Guid CartId)
        {
            var cart = _cartRepo.GetCart(CartId);
            if(cart != null)
            {
                return cart.CartItems.Count();
            }
            return 0;
        }

        public CartModel GetCartDetails(Guid CartId)
        {
            var model = _cartRepo.GetCartDetails(CartId);
            if (model != null)
            {
                decimal subTotal = 0;
                foreach (var item in model.Items)
                {
                    item.Total = item.UnitPrice * item.Quantity;
                    subTotal += item.Total;
                }
                model.Total = subTotal;
                model.Tax = (subTotal * Convert.ToDecimal(_config["Tax:GST"])) / 100;
                model.GrandTotal = model.Total + model.Tax;

            }
            return model;
        }

        public int UpdateCart(Guid CartId, int UserId)
        {
            return _cartRepo.UpdateCart(CartId, UserId);
        }

        public int UpdateQuantity(Guid CartId, int ItemId, int Quantity)
        {
            return _cartRepo.UpdateQuantity(CartId, ItemId, Quantity);
        }
    }
}
