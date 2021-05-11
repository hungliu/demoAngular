using Demo3.DAL.Interface;
using Demo3.DAL.Repositories;
using Demo3.Data;
using Demo3.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo3.DAL.Services
{
    public class ProductService : Repository<Product>, IProductService
    {
        public ProductService(DemoDbContext db) : base(db)
        {
        }

        public IEnumerable<Product> GetAllProduct()
        {
            return GetAll();
        }

        public Product GetInfoProductById(int id)
        {
            return GetInfoByID(id);
        }

        public async Task<bool> AddProduct(Product product)
        {
            Product product1 = GetInfoByID(product.ID);
            if (product1 != null)
            {
                return false;
            }
            await Insert(product);
            return true;
        }

        

        public async Task<bool> UpdateInfoProduct(int id, Product info)
        {
            Product product = GetInfoByID(id);
            if (product != null)
            {
                product.ID = info.ID;
                product.ProductName = info.ProductName;
                product.Image = info.Image;
                product.Quantity = info.Quantity;
                product.Price = info.Price;
                product.Brand = info.Brand;
                product.Description = info.Description;
                product.CategoryID = info.CategoryID;
                await Update(product);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            Product category = GetInfoByID(id);
            if (category == null)
            {
                return false;
            }
            await Delete(category);
            return true;
        }

        public IEnumerable<Product> GetProductsByBrand(string brand)
        {
            var list = (from a in _db.Products
                        join b in _db.Categories on a.CategoryID equals b.CategoryID
                        where a.Brand == brand
                        select new
                        {
                            ID = a.ID,
                            ProductName = a.ProductName,
                            Image = a.Image,
                            Quantity = a.Quantity,
                            Price = a.Price,
                            Description = a.Description,
                            Brand = a.Brand,
                            CreatedBy = a.CreatedBy,
                            CreatedTime = a.CreatedTime,
                            UpdatedBy = a.UpdatedBy,
                            UpdatedTime = a.UpdatedTime,
                            DeletedBy = a.DeletedBy,
                            DeletedTime = a.DeletedTime,
                            CategoryID = a.CategoryID,
                        });
            List<Product> listProduct = new List<Product>();
            foreach (var item in list.ToList())
            {
                Product product = new Product();
                product.ID = item.ID;
                product.ProductName = item.ProductName;
                product.Image = item.Image;
                product.Quantity = item.Quantity;
                product.Price = item.Price;
                product.Description = item.Description;
                product.Brand = item.Brand;
                product.CreatedBy = item.CreatedBy;
                product.CreatedTime = item.CreatedTime;
                product.UpdatedBy = item.UpdatedBy;
                product.UpdatedTime = item.UpdatedTime;
                product.DeletedBy = item.DeletedBy;
                product.DeletedTime = item.DeletedTime;
                product.CategoryID = item.CategoryID;
                listProduct.Add(product);
            }
            return listProduct;
        }

    }
}
