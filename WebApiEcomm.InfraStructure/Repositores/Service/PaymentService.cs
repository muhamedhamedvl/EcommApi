using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiEcomm.Core.Entites.Basket;
using WebApiEcomm.Core.Interfaces.IUnitOfWork;
using WebApiEcomm.Core.Services;
using WebApiEcomm.InfraStructure.Data;

namespace WebApiEcomm.InfraStructure.Repositores.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        public PaymentService(IUnitOfWork unitOfWork, IConfiguration configuration, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _context = context;
        }
        public async Task<CustomerBasket> CreateOrUpdatePaymentAsync(string basketId , int deliveryMethodId)
        {
            var baseket = _unitOfWork.CustomerBasketRepository.GetCustomerBasketAsync(basketId);
            StripeConfiguration.ApiKey = _configuration["StripeSetting : SecretKey"];
            var ShippingPrice = 0m;
            if (deliveryMethodId > 0)
            {
                var deliveryMethod = _context.DeliveryMethods.AsNoTracking().FirstOrDefault(x => x.Id == deliveryMethodId);
                ShippingPrice = deliveryMethod?.Price ?? 0m;
            }
            foreach (var item in baseket.Result.basketItems)
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.Id);
                item.Price = (int)product.NewPrice;
            }
            PaymentIntent paymentIntent = new PaymentIntentService().Create(new PaymentIntentCreateOptions
            {
                Amount = (long)(baseket.Result.basketItems.Sum(x => x.Price * x.Quantity) + ShippingPrice) * 100,
                Currency = "usd",
                Description = "Payment for order",
                Metadata = new Dictionary<string, string>
                {
                    { "BasketId", basketId }
                }
            });
            baseket.Result.PaymentIntentId = paymentIntent.Id;
            baseket.Result.ClientSecret = paymentIntent.ClientSecret;
            baseket.Result.basketItems.ForEach(x => x.Price = (int)x.Price);
            if (baseket.Result.basketItems.Count == 0)
            {
                _unitOfWork.CustomerBasketRepository.DeleteCustomerBasketAsync(basketId);
                return null;
            }
            else
            {
                await _unitOfWork.CustomerBasketRepository.UpdateCustomerBasketAsync(baseket.Result);
                await _context.SaveChangesAsync();
                return baseket.Result;
            }
        }
    }
}
