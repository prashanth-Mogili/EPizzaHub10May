using ePizzaHub.Core.Entities;
using ePizzaHub.Models;
using ePizzaHub.Services.Interfaces;
using ePizzaHub.UI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.UI.Controllers
{
    public class CartController : BaseController
    {
        ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        Guid CartId
        {
            get
            {
                if (HttpContext.Request.Cookies["CId"] == null)
                {
                    Guid cartId = Guid.NewGuid();
                    HttpContext.Response.Cookies.Append("CId", cartId.ToString());
                    return cartId;
                }
                else
                {
                    return Guid.Parse(HttpContext.Request.Cookies["CId"]);
                }
            }
        }
        public IActionResult Index()
        {
            CartModel cartModel = _cartService.GetCartDetails(CartId);
            return View(cartModel);
        }

        [Route("Cart/AddToCart/{ItemId}/{UnitPrice}/{Quantity}")]

        public IActionResult AddToCart(int ItemId, decimal UnitPrice, int Quantity)
        {
            int userId = CurrentUser!=null? CurrentUser.Id : 0;
            Cart cart = _cartService.AddItem(userId, CartId, ItemId, UnitPrice, Quantity);
            if(cart!=null)
            {
                return Json(new {status = "success", count = cart.CartItems.Count});
            }
            return Json(new { status = "failed", count = 0 });
        }

        [Route("Cart/updateQuantity/{ItemId}/{Quantity}")]
        public IActionResult UpdateQuantity(int ItemId, int Quantity)
        {
            int result = _cartService.UpdateQuantity(CartId, ItemId, Quantity);
            return Json(result);
        }

        [Route("Cart/DeleteItem/{ItemId}")]
        public IActionResult DeleteItem(int ItemId)
        {
            int result = _cartService.DeleteItem(CartId, ItemId);
            return Json(result);
        }

        public IActionResult GetCartCount()
        {
            if (CartId != null)
            {
                int count = _cartService.GetCartCount(CartId);
                return Json(count);
            }
            return Json(0);
        }

        public IActionResult CheckOut()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CheckOut(AddressModel model)
        {
            if (ModelState.IsValid)
            {
                CartModel cartModel = _cartService.GetCartDetails(CartId);
                if(CurrentUser!=null && cartModel != null) { 
                    cartModel.UserId = CurrentUser.Id;
                    _cartService.UpdateCart(cartModel.Id, CurrentUser.Id);
                }
                TempData.Set("Address", model);
                TempData.Set("Cart", model);
                return RedirectToAction("Index","Payment");
            }
            return View();
        }
    }
}
