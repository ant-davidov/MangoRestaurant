using Mango.Services.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Product> Products => Set<Product>();
        public ApplicationDbContext() => Database.EnsureCreated();
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


    }

}
