using Mango.Services.ShoppingCartAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartAPI.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
         public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public ApplicationDbContext() => Database.EnsureCreated();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<CartHeader> CartHeaders => Set<CartHeader>();
        public DbSet<CartDetails> CartDetails => Set<CartDetails>();
    }
}
