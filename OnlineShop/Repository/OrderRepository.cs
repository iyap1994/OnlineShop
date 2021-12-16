using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Context;
using OnlineShop.Model;

namespace OnlineShop.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OnlineShopDbContext _onlineShopDbContext;

        public OrderRepository(OnlineShopDbContext onlineShopDbContext)
        {
            _onlineShopDbContext = onlineShopDbContext;
        }

        public Order PlaceOrder(Order product)
        {
            _onlineShopDbContext.Orders.Add(product);
            _onlineShopDbContext.SaveChanges();
            return product;
        }

        public IEnumerable<Order> GetOrder()
        {
            return _onlineShopDbContext.Orders.ToList();
        }

        public Order GetOrder(int id)
        {
            return _onlineShopDbContext.Orders.FirstOrDefault(x => x.OrderId == id);
        }

        public void UpdateOrder(int id, Order product)
        {
            _onlineShopDbContext.Orders.Update(product);
            _onlineShopDbContext.SaveChanges();
        }

        public void CancelOrder(Order product)
        {
            _onlineShopDbContext.Orders.Remove(product);
            _onlineShopDbContext.SaveChanges();
        }
    }


    public interface IOrderRepository
    {
        Order PlaceOrder(Order product);
        IEnumerable<Order> GetOrder();
        Order GetOrder(int id);
        void UpdateOrder(int id, Order product);
        void CancelOrder(Order product);
    }
}
