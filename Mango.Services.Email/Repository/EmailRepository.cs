using Mango.Services.Email.DbContexts;
using Mango.Services.Email.Messages;
using Mango.Services.Email.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Mango.Services.Email.Repository
{
    public class EmailRepository : IEmailRepository
    {

        private readonly DbContextOptions<ApplicationDbContext> _context;

        public EmailRepository(DbContextOptions<ApplicationDbContext> context)
        {
            _context = context;
        }

        public async Task SendAndLogEmail(UpdatepaymentResultMessage message)
        {
            //ToDo;
            EmailLog emailLog = new EmailLog()
            {
                Email = message.Email,
                EmailSent = DateTime.UtcNow,
                Log = $"Order - {message.OrderId} has been created successfully"
            };

            var _db = new ApplicationDbContext(_context);
            _db.EmailLogs.Add(emailLog);
            await _db.SaveChangesAsync();
        }
    }
}
