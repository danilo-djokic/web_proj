using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CafeManager.Models;
using CafeManager.Persistence_Contexts;

namespace CafeManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext Context;
        internal DbSet<Product> dbSet;
        public ProductController(AppDbContext context){
            Context = context;
            dbSet = Context.Product;
        }

        [Route("")]
        [HttpGet]
        public async Task<ActionResult> FindAll(){
            try{
                return Ok(await Context.Product.ToListAsync());
            }
            catch (Exception e){
                return BadRequest(e.Message);
            }
        }
        [Route("/{id}")]
        [HttpGet]
        public async Task<ActionResult> FindById(int id){
            try{
                var Product = await Context.Product.FirstOrDefaultAsync(p => p.Id == id );
                if(Product == null){
                    return BadRequest("Product not found.");
                }
                return Ok(Product);
            }
            catch(Exception e){
                return BadRequest(e.Message);
            }
        }

        [Route("")]
        [HttpPost]
        public async Task<ActionResult> Save([FromBody] Product product){
            try{
                await dbSet.AddAsync(product);
                Context.SaveChanges();
                return Ok(product);
            }
            catch (Exception e){
                return BadRequest(e.Message);
            }
        }

        [Route("")]
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] Product product){
            try{
                var existingProduct = await dbSet.FirstOrDefaultAsync(p=> p.Id == product.Id);
                if(existingProduct ==null)
                    return BadRequest("No product with such Id");

                existingProduct.ProductName = product.ProductName;
                existingProduct.Price = product.Price;
                
                Context.SaveChanges();
                return Ok(product);
            }
            catch (Exception e){
                return BadRequest(e.Message);
            }
        }

        [Route("")]
        [HttpDelete]
        public ActionResult Delete([FromBody] Product product){
            try{
                dbSet.Remove(product);
                Context.SaveChanges();
                return Ok("Successfully deleted product with id: " + product.Id);
            }
            catch (Exception e){
                return BadRequest(e.Message);
            }
        }
    }
}