using Mango.Services.ShoppingCartAPI.Model.DTO;

namespace Mango.Services.ShoppingCartAPI.Repository
{
    public interface ICartRepository
    {
        Task<CartDTO> GetCartByUserIdAsync(string userId);
        Task<CartDTO> CreateUpdateCartAsync(CartDTO cartDTO);
        Task<bool> RemoveFromCartAsync(int cartDetailsId);
        Task<bool> ClearCartAsync(string userId);
        Task<bool> ApplyCoupon(string userId, string couponCode);
        Task<bool> RemoveCoupon(string userId);
    }
}
