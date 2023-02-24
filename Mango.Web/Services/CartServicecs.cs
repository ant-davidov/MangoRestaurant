using Mango.Web.Models;
using Mango.Web.Services.IServices;

namespace Mango.Web.Services
{
    public class CartServicecs : BaseService ,ICartService
    {
        public CartServicecs(IHttpClientFactory httpClient) : base(httpClient)
        {
        }

        public Task<T> AddCartIdAsync<T>(CartDTO cartDTO, string token = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetCartByUserIdAsync<T>(string userId, string token = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> RemoveFromCartAsync<T>(int cartId, string token = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> UpdateCartAsync<T>(CartDTO cartDTO, string token = null)
        {
            throw new NotImplementedException();
        }
    }
}
