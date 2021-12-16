using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Context;
using OnlineShop.Model;

namespace OnlineShop.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly OnlineShopDbContext _onlineShopDbContext;

        public ProductRepository(OnlineShopDbContext onlineShopDbContext)
        {
            _onlineShopDbContext = onlineShopDbContext;
        }

        public void AddProduct(Product product)
        {
            _onlineShopDbContext.Products.Add(product);
            _onlineShopDbContext.SaveChanges();
        }

        public IEnumerable<Product> GetProduct()
        {
            return _onlineShopDbContext.Products.ToList();
        }

        public Product GetProduct(int productId)
        {
            return _onlineShopDbContext.Products.FirstOrDefault(x => x.ProductId == productId);
        }

        public void UpdateProduct(int id, Product product)
        {
            _onlineShopDbContext.Products.Update(product);
            _onlineShopDbContext.SaveChanges();
        }

        public void RemoveProduct(Product product)
        {
            _onlineShopDbContext.Products.Remove(product);
            _onlineShopDbContext.SaveChanges();
        }

    }

    public interface IProductRepository
    {
        void AddProduct(Product product);
        IEnumerable<Product> GetProduct();
        Product GetProduct(int id);
        void UpdateProduct(int id, Product product);
        void RemoveProduct(Product product);
    }
}
