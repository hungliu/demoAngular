using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Demo3.Data;
using Demo3.Models;
using Microsoft.AspNetCore.Authorization;
using Demo3.DAL.Services;
using Demo3.Middleware;

namespace Demo3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductsController : ControllerBase
    {
        private ProductService _serProduct;

        public ProductsController(ProductService productService)
        {
            _serProduct = productService;
        }

        //[JwtAuthAtrribute]
        [HttpGet]
        public IEnumerable<Product> GetProducts()
        {
            return  _serProduct.GetAllProduct();
        }

        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            var product =  _serProduct.GetInfoProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            try
            {
                if (id != product.ID)
                {
                    return BadRequest();
                }
                var resp = await _serProduct.UpdateInfoProduct(id, product);
                if (resp == true)
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
                //if (!ProductExists(id))
                //{
                //    return NotFound();
                //}
                //else
                //{
                //    throw;
                //}
                return StatusCode(500);
            }

            //return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            var resp = await _serProduct.AddProduct(product);
            if (resp == true)
            {
                return CreatedAtAction("GetProduct", new { id = product.ID }, product);
            }
            return Unauthorized();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var resp = await _serProduct.DeleteProduct(id);
            if(resp == true)
            {
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
            //var product =  _db.Products.FindAsync(id);
            //if (product == null)
            //{
            //    return NotFound();
            //}

            //_db.Products.Remove(product);
            // _db.SaveChangesAsync();

            //return NoContent();
        }



        //[HttpPost("FindByBrand")]
        //public async Task<IActionResult> FindByBrand(string brand)
        //{
        //    try
        //    {
        //        return Ok( _serProduct.GetProductsByBrand(brand));
        //    }
        //    catch
        //    {
        //        return StatusCode(500);
        //    }
        //}
    }
}
