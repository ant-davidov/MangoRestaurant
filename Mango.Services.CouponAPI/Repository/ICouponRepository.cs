using Mango.Services.CouponAPI.Model.DTO;

namespace Mango.Services.CouponAPI.Repository
{
    public interface ICouponRepository
    {
        Task<CouponDTO> GetCouponByCode(string code);
    }
}
