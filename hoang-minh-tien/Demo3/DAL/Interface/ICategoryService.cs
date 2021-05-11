using Demo3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo3.DAL.Interface
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAllCategory();
        public Category GetInfoCategoryById(int id);
        public Task<bool> UpdateInfoCategory(int id, Category info);
        public Task<bool> AddCategory(Category category);
        public Task<bool> DeleteCategory(int id);
    }
}
