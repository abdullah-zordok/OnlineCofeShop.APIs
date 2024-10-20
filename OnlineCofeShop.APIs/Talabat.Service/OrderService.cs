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
using Talabat.Core.Specifications;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        //private readonly IGenericRepository<Product> _productsRepo;
        //private readonly IGenericRepository<DeliveryMethod> _deliveryMethod;
        //private readonly IGenericRepository<Order> _orderRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepo ,IUnitOfWork unitOfWork,IPaymentService paymentService/*IGenericRepository<Product> productsRepo,IGenericRepository<DeliveryMethod> deliveryMethod,IGenericRepository<Order> orderRepo*/)
        {
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            _basketRepo = basketRepo;
            //_productsRepo = productsRepo;
            //_deliveryMethod = deliveryMethod;
            // _orderRepo = orderRepo;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            //1- GET BASKET FROM BASKET REPO

            var basket = await _basketRepo.GetBasketAsync(basketId);

            //2- Get Selected Items at Basket From Products Repo

            var orderItems = new List<OrderItems>();

            if (basket?.Items?.Count > 0)
            {
                var productRepository=_unitOfWork.Repository<Product>();

                foreach (var item in basket.Items) {

                    var product = await productRepository.GetByIdAsync(item.Id);

                    var productItemOrdered = new ProductItemOrdered(item.Id,product.Name,product.PictureUrl);

                    var orderItem = new OrderItems(productItemOrdered, product.Price, item.Quantity);

                    orderItems.Add(orderItem);
                    
                  }
            }
            //3- Calculate SubTotal
            var subtotal = orderItems.Sum(orderItem => orderItem.Price *orderItem.Quantity);

            //4- Get Delivery Method From Delivery Methods Repo

            var deliveryMethod=await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            var ordersRepo=_unitOfWork.Repository<Order>();

            var orderSpecs = new OrderWithPaymentIntentSpecifications(basket.PaymentIntentId);

            var existingOrder=await ordersRepo.GetByEntityWithSpecAsync(orderSpecs);

            if(existingOrder != null)
            {
               ordersRepo.DeleteAsync(existingOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basketId);

            }
            //5- Create Order
            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subtotal,basket.PaymentIntentId);

            await _unitOfWork.Repository<Order>().AddAsync(order);

            //6- save to database
           var result= await _unitOfWork.CompleteAsync();
            if (result <= 0) return null;
            return order;
        }
        public Task<Order?> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var orderRepo=_unitOfWork.Repository<Order>();
            var orderSpec = new OrderSpecifications(orderId, buyerEmail);
            var order= orderRepo.GetByEntityWithSpecAsync(orderSpec);
            return order;

        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var ordersRepo = _unitOfWork.Repository<Order>();
            var spec = new OrderSpecifications(buyerEmail);
            var orders= await ordersRepo.GetAllWithSpecAsync(spec);
            return orders;
        }
   

        Task<IReadOnlyList<DeliveryMethod>> IOrderService.GetDeliveryMethodsAsync()
        {
            var deliveryMethodsRepo = _unitOfWork.Repository<DeliveryMethod>();
            var deliveryMethods = deliveryMethodsRepo.GetAllAsync();
            return deliveryMethods;
        }
    }
}
