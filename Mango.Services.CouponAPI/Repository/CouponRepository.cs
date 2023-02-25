using AutoMapper;
using Mango.Services.CouponAPI.DbContexts;
using Mango.Services.CouponAPI.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public CouponRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CouponDTO> GetCouponByCode(string code)
        {
            var coupon = await _context.Coupons.FirstOrDefaultAsync(u=>u.CouponCode== code);
            return _mapper.Map<CouponDTO>(coupon);
        }
    }
}
