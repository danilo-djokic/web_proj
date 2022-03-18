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
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext Context;
        internal DbSet<Admin> dbSet;

        public AdminController(AppDbContext context){
            Context = context;
            dbSet = context.Admin;
        }
        [Route("")]
        [HttpGet]
        public async Task<ActionResult> FindAll(){
            try{
                return Ok(await dbSet.ToListAsync());
            }
            catch (Exception e){
                return BadRequest(e.Message);
            }
        }
        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult> FindById(int id){
            try{
                var admin = await Context.Admin.Where(p => p.Id == id ).FirstOrDefaultAsync();
                if(admin == null){
                    return BadRequest("Admin not found.");
                }
                return Ok(admin);
            }
            catch(Exception e){
                return BadRequest(e.Message);
            }
        }

        [Route("")]
        [HttpPost]
        public async Task<ActionResult> Save([FromBody] Admin admin){
            try{
                await dbSet.AddAsync(admin);
                Context.SaveChanges();
                return Ok(admin);
            }
            catch (Exception e){
                return BadRequest(e.Message);
            }
        }

        [Route("")]
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] Admin admin){
            try{
                var existingAdmin = await dbSet.FirstOrDefaultAsync(p=> p.Id == admin.Id);
                if(existingAdmin ==null)
                    return BadRequest("No admin with such Id");

                existingAdmin.Username = admin.Username;
                existingAdmin.Password = admin.Password;
                
                Context.SaveChanges();
                return Ok(admin);
            }
            catch (Exception e){
                return BadRequest(e.Message);
            }
        }

        [Route("")]
        [HttpDelete]
        public ActionResult Delete([FromBody] Admin admin){
            try{
                dbSet.Remove(admin);
                Context.SaveChanges();
                return Ok("Successfully deleted admin with id: " + admin.Id);
            }
            catch (Exception e){
                return BadRequest(e.Message);
            }
        }
    }
}