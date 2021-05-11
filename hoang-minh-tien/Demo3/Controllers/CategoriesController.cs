using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Demo3.Data;
using Demo3.Models;
using Demo3.DAL.Services;

namespace Demo3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private CategoryService _serCategory;
        private ProductService _serProduct;

        public CategoriesController(CategoryService serCategory, ProductService serProduct)
        {
            _serCategory = serCategory;
            _serProduct = serProduct;
        }

        [HttpGet]
        public IEnumerable<Category> GetCategories()
        {
            return  _serCategory.GetAllCategory();
        }

        [HttpGet("{id}")]
        public ActionResult<Category> GetCategory(int id)
        {
            var category =  _serCategory.GetInfoCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoryAsync(int id, Category category)
        {
            try
            {
                if (id != category.CategoryID)
                {
                    return BadRequest();
                }
                var resp = await _serCategory.UpdateInfoCategory(id, category);
                if(resp == true)
                {
                    return Ok();
                }
                else
                {
                    return Unauthorized();
                }
                
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500);
            }

            //return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            var resp = await _serCategory.AddCategory(category);
            if(resp == true)
            {
                return CreatedAtAction("GetCategory", new { id = category.CategoryID }, category);
            }
            return Unauthorized();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var resp = await _serCategory.DeleteCategory(id);
            if(resp == true)
            {
                return Ok();
            }
            return Unauthorized();
            //var category =  _context.Categories.FindAsync(id);
            //if (category == null)
            //{
            //    return NotFound();
            //}
            //_context.Categories.Remove(category);
            // _context.SaveChangesAsync();

            //return category;
        }

    }
}
