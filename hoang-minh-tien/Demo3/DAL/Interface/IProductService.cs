using Demo3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo3.DAL.Interface
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProduct();
        public Product GetInfoProductById(int id);
        public Task<bool> UpdateInfoProduct(int id, Product info);
        public Task<bool> AddProduct(Product product);
        public Task<bool> DeleteProduct(int id);
        public IEnumerable<Product> GetProductsByBrand(string brand);
    }
}
