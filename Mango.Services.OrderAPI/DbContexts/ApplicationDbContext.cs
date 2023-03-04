using Mango.Services.OrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.OrderAPI.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
         public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public ApplicationDbContext() => Database.EnsureCreated();
        public DbSet<OrderHeader> OrderHeaders => Set<OrderHeader>();
        public DbSet<OrderDetails> OrderDetails => Set<OrderDetails>();

    }
}
