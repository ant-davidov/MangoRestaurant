using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Mango.Services.Email.DbContexts;

namespace Mango.Services.Email
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlite("Data Source=EmailLogDb.db");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
