using Microsoft.EntityFrameworkCore;
using CafeManager.Models;
namespace CafeManager.Persistence_Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Table> Table { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=localhost;Database=CafeManagerDb;Username=postgres;Password=asd");
    }
}