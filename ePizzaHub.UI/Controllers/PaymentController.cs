using ePizzaHub.Core.Entities;
using ePizzaHub.Models;
using ePizzaHub.Services.Interfaces;
using ePizzaHub.UI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.UI.Controllers
{
    public class PaymentController : BaseController
    {
        IPaymentService _paymentService;
        IConfiguration _configuration;
        IOrderService _orderService;

        public PaymentController(IPaymentService paymentService, IConfiguration configuration, IOrderService orderService)
        {
            _paymentService = paymentService;
            _configuration = configuration;
            _orderService = orderService;

        }

        public IActionResult Index()
        {
            PaymentModel paymentModel = new PaymentModel();
            CartModel cart = TempData.Peek<CartModel>("Cart");
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

        [HttpPost]
        public IActionResult Status(IFormCollection form)
        {
            try
            {
                if (form.Keys.Count > 0)
                {
                    string orderID = form["rzp_orderid"];
                    string paymentId = form["rzp_paymentid"];
                    string signature = form["rzp_signature"];
                    string transactionId = form["Receipt"];
                    string currency = form["Currency"];

                    bool isValid = _paymentService.VerifySignature(signature, orderID, paymentId);
                    if (isValid)
                    {
                        CartModel cart = TempData.Peek<CartModel>("Cart");
                        PaymentDetail model = new PaymentDetail();

                        model.CartId = cart.Id;
                        model.Total = cart.Total;
                        model.Tax = cart.Tax;
                        model.Currency = currency;
                        model.GrandTotal = cart.GrandTotal;
                        model.CreatedDate = DateTime.Now;

                        model.Status = "Success";
                        model.TransactionId = transactionId;
                        model.Id = paymentId;
                        model.Email = CurrentUser.Email;
                        model.UserId = CurrentUser.Id;

                        int status = _paymentService.SavePaymentDetails(model);
                        if(status > 0)
                        {
                            Response.Cookies.Delete("CID");
                            TempData.Remove("Cart");

                            AddressModel addressModel = TempData.Get<AddressModel>("Address");
                            _orderService.PlaceOrder(CurrentUser.Id, orderID, paymentId, cart, addressModel);

                            TempData.Set("PaymentDetails", model);
                            return RedirectToAction("Receipt");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Your payment has been failed. Please contact us at support@epizzahub.com";
            }

            return View();
        }

        public IActionResult Receipt()
        {
            PaymentDetail paymentDetail = TempData.Get<PaymentDetail>("PaymentDetails");
            return View(paymentDetail);
        }

    }
}
