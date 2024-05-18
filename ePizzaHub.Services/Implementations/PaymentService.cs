using ePizzaHub.Core.Entities;
using ePizzaHub.Repositories.Interfaces;
using ePizzaHub.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Order = Razorpay.Api.Order;

namespace ePizzaHub.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        RazorpayClient _client;
        IConfiguration _configuration;
        IRepository<PaymentDetail> _paymentRepo;
        ICartRepository _cartRepo;
        public PaymentService(IConfiguration configuration, IRepository<PaymentDetail> paymentRepo, ICartRepository cartRepo)
        {
            _configuration = configuration;
            _client = new RazorpayClient(_configuration["RazorPay:Key"], _configuration["RazorPay:Secret"]);
            _paymentRepo = paymentRepo;
            _cartRepo = cartRepo;
        }
        public string CreateOrder(decimal amount, string currency, string receipt)
        {
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", 50000); // amount in the smallest currency unit
            options.Add("receipt", "order_rcptid_11");
            options.Add("currency", "INR");
            Order order = _client.Order.Create(options);
            return order["id"].ToString();
        }

        public Payment GetPaymentDetails(string paymentId)
        {
           return _client.Payment.Fetch(paymentId);
        }

        public int SavePaymentDetails(PaymentDetail model)
        {
            _paymentRepo.Add(model);
            Cart cart = _cartRepo.GetCart(model.CartId);
            cart.IsActive = false;
            return _paymentRepo.SaveChanges();
        }

        public bool VerifySignature(string signature, string orderId, string paymentId)
        {
           string payload = string.Format("{0}|{1}",orderId,paymentId);
            string secret = RazorpayClient.Secret;
            string actualSignature = getActualSignature(payload, secret);
            return actualSignature.Equals(signature);
        }

        private static string getActualSignature(string payload, string secret) {
            byte[] secretBytes = StringEncode(secret);
            HMACSHA256 hashHmac = new HMACSHA256(secretBytes);
            var bytes = StringEncode(payload);
            return HashEncode(hashHmac.ComputeHash(bytes));
        }

        private static string HashEncode(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-","").ToLower();
        }

        private static byte[] StringEncode(string text)
        {
            var encoding = new ASCIIEncoding();
            return encoding.GetBytes(text);
        }
    }
}
