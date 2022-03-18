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
    public class TableController : ControllerBase
    {
        private readonly AppDbContext Context;
        internal DbSet<Table> dbSet;
        public TableController(AppDbContext context){
            Context = context;
            dbSet = Context.Table;
        }

        [Route("")]
        [HttpGet]
        public async Task<ActionResult> FindAll(){
            try{
                return Ok(await Context.Table.ToListAsync());
            }
            catch (Exception e){
                return BadRequest(e.Message);
            }
        }
        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult> FindById(int id){
            try{
                var Table = await dbSet.Include("Products").AsNoTracking().FirstOrDefaultAsync(p => p.Id == id );
                if(Table == null){
                    return BadRequest("Table not found.");
                }
                return Ok(Table);
            }
            catch(Exception e){
                return BadRequest(e.Message);
            }
        }

        [Route("")]
        [HttpPost]
        public async Task<ActionResult> Save([FromBody] Table table){
            try{
                await dbSet.AddAsync(table);
                Context.SaveChanges();
                return Ok(table);
            }
            catch (Exception e){
                return BadRequest(e.Message);
            }
        }

        [Route("")]
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] Table table){
            try{
                var existingTable = await dbSet.FirstOrDefaultAsync(p=> p.Id == table.Id);
                if(existingTable ==null)
                    return BadRequest("No table with such Id");

                existingTable.NoOfSeats = table.NoOfSeats;
                existingTable.Occupied = table.Occupied;
                existingTable.AdminId = table.AdminId;
                
                Context.SaveChanges();
                return Ok(table);
            }
            catch (Exception e){
                return BadRequest(e.Message);
            }
        }

        [Route("")]
        [HttpDelete]
        public ActionResult Delete([FromBody] Table table){
            try{
                dbSet.Remove(table);
                Context.SaveChanges();
                return Ok("Successfully deleted table with id: " + table.Id);
            }
            catch (Exception e){
                return BadRequest(e.Message);
            }
        }

        [Route("addProduct/{id}")]
        [HttpPost]
        public async Task<ActionResult> AddProductToTable(int id, [FromBody] Product product){
            try{
                var existingTable = await dbSet.Include("Products").FirstOrDefaultAsync(p => p.Id == id);
                if(existingTable == null)
                    return BadRequest("No Table with such id");

                if(existingTable.Products== null)
                    existingTable.Products = new List<Product>();
                if(!existingTable.Products.Contains(product))
                    existingTable.Products.Add(product);

                Context.SaveChanges();
                return Ok("Successfully added product to table");
            }
            catch (Exception e){
                return BadRequest(e.InnerException);
            }
        }

        [Route("removeProduct/{id}/{productId}")]
        [HttpDelete]
        public async Task<ActionResult> RemoveProductToTable(int id, int productId){
            try{
                var existingTable = await dbSet.Include("Products").FirstOrDefaultAsync(p => p.Id == id);
                if(existingTable == null)
                    return BadRequest("No Table with such id");
                if(existingTable.Products!= null){
                    Product product = existingTable.Products.Single(p=> p.Id == productId);
                    existingTable.Products.Remove(product);
                    Context.SaveChanges();
                    return Ok(existingTable);
                }
                return BadRequest("No product with such id");
            }
            catch (Exception e){
                return BadRequest(e.Message);
            }
        }

    }
}