using Mango.Services.ShoppingCartAPI.Model.DTO;

namespace Mango.Services.ShoppingCartAPI.Repository
{
    public interface ICouponRepository
    {
        Task<CouponDTO> GetCoupon(string couponName);
    }
}
