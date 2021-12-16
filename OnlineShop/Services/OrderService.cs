using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using OnlineShop.Model;
using OnlineShop.Repository;

namespace OnlineShop.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IServiceProvider _provider;

        public OrderService(IServiceProvider provider, IOrderRepository orderRepository)
        {
            _provider = provider;
            _orderRepository = orderRepository;
        }

        public IEnumerable<Order> GetOrder()
        {
            return _orderRepository.GetOrder();
        }

        public Order GetOrder(int id)
        {
            return _orderRepository.GetOrder(id);
        }

        public Order PlaceOrder(Order order)
        {
            int availableQty = _provider.GetRequiredService<IProductService>().GetQuantity(order.ProductId);

            if (availableQty < order.Quantity) throw new Exception($"Only {availableQty} stock available");

            _orderRepository.PlaceOrder(order);

            int remainingQty = availableQty - order.Quantity;

            _provider.GetRequiredService<IProductService>().UpdateQuantity(order.ProductId, remainingQty);

            return order;
        }

        public void CancelOrder(Order order)
        {
            try
            {
                _orderRepository.CancelOrder(order);
                int availableQty = _provider.GetRequiredService<IProductService>().GetQuantity(order.ProductId);
                _provider.GetRequiredService<IProductService>().UpdateQuantity(order.ProductId, availableQty + order.Quantity);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void CancelOrder(int orderId)
        {
            try
            {
                var order = GetOrder(orderId);
                CancelOrder(order);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }

    public interface IOrderService
    {
        IEnumerable<Order> GetOrder();
        Order GetOrder(int id);
        Order PlaceOrder(Order order);
        void CancelOrder(Order order);
        void CancelOrder(int orderId);
    }
}