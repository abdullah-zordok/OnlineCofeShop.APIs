using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;

namespace Talabat.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepo;
        private readonly IConfiguration _configuration;

        public PaymentService(
            IUnitOfWork unitOfWork,
            IBasketRepository BasketRepo,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _basketRepo = BasketRepo;
            _configuration = configuration;
        }

        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId)
        {

            //1.Set Secret Key 
            StripeConfiguration.ApiKey = _configuration["StripeSettings:Secretkey"];

            var basket = await _basketRepo.GetBasketAsync(basketId);

            if (basket is null) return null;

            var ShippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue)
            {

                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                basket.ShippingPrice = deliveryMethod.Cost;
                ShippingPrice = deliveryMethod.Cost;

            }

            //2.check From Price is True 
            if (basket?.Items.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var Product = await _unitOfWork.Repository<Core.Entities.Product>().GetByIdAsync(item.Id);

                    if (item.Price != Product.Price)
                        item.Price = Product.Price;
                }
            }


            //3.USe PaymentIntentService
            PaymentIntentService paymentIntentService = new PaymentIntentService();

            PaymentIntent paymentIntent;

            //Create New Payment Intent 
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var Createoptions = new PaymentIntentCreateOptions()
                {

                    Amount = (long)basket.Items.Sum(item => item.Price * 100 * item.Quantity) + (long)ShippingPrice * 100,

                    Currency = "usd",

                    PaymentMethodTypes = new List<string>() { "card" }
                };

                //API Integrate(Interact) With Stripe Through secretKey
                paymentIntent = await paymentIntentService.CreateAsync(Createoptions);

                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }

            //Update Existing Payment Intent
            else
            {
                var UpdateOptions = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * 100 * item.Quantity) + (long)ShippingPrice * 100

                };

                await paymentIntentService.UpdateAsync(basket.PaymentIntentId, UpdateOptions);


            }

            await _basketRepo.UpdateBasketAsync(basket);

            return basket;

        }
    }
}
