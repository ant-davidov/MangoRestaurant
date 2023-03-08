using Mango.Services.Email.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.Email.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public ApplicationDbContext() => Database.EnsureCreated();
        public DbSet<EmailLog> EmailLogs => Set<EmailLog>();    

    }
}
