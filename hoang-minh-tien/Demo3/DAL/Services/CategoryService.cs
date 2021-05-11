using Demo3.DAL.Interface;
using Demo3.DAL.Repositories;
using Demo3.Data;
using Demo3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo3.DAL.Services
{
    public class CategoryService : Repository<Category>, ICategoryService
    {
        public CategoryService(DemoDbContext db) : base(db) { }

        public IEnumerable<Category> GetAllCategory()
        {
            return GetAll();
        }

        public Category GetInfoCategoryById(int id)
        {
            return GetInfoByID(id);
        }

        public async Task<bool> AddCategory(Category category)
        {
            Category category1 = GetInfoByID(category.CategoryID);
            if (category1 != null)
            {
                return false;
            }
            await Insert(category);
            return true;
        }

        public async Task<bool> UpdateInfoCategory(int id, Category info)
        {
            Category category = GetInfoByID(id);
            if (category == null)
            {
                return false;
            }
            category.CategoryID = info.CategoryID;
            category.CategoryName = info.CategoryName;
            await Update(category);
            return true;

        }

        public async Task<bool> DeleteCategory(int id)
        {
            Category category = GetInfoByID(id);
            if (category == null)
            {
                return false;
            }
            await Delete(category);
            return true;
        }
    }
}
