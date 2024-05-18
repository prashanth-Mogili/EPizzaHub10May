using ePizzaHub.Models;
using ePizzaHub.Services.Interfaces;
using ePizzaHub.UI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.UI.Controllers
{
    public class PaymentController : Controller
    {
        IPaymentService _paymentService;
        IConfiguration _configuration;

        public PaymentController(IPaymentService paymentService, IConfiguration configuration)
        {
            _paymentService = paymentService;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            PaymentModel paymentModel = new PaymentModel();
            CartModel cart = TempData.Get<CartModel>("Cart");
            if(cart != null)
            {
                paymentModel.Cart = cart;
                paymentModel.GrandTotal = cart.GrandTotal;
                paymentModel.Currency = "INR";
                paymentModel.Description = string.Join(", ", cart.Items.Select(x => x.Name));
                paymentModel.Receipt = Guid.NewGuid().ToString();

                paymentModel.RazorpayKey = _configuration["Razorpay:Key"];
                paymentModel.OrderId = _paymentService.CreateOrder
                    (paymentModel.GrandTotal, paymentModel.Currency, paymentModel.Receipt);

            }

            return View(paymentModel);
        }
    }
}
