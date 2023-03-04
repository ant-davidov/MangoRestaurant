using Mango.Web.Models;
using Mango.Web.Services.IServices;

namespace Mango.Web.Services
{
    public class CouponService :BaseService,ICouponService
    {
        public CouponService(IHttpClientFactory httpClient) : base(httpClient)
        {
        }
        public async Task<T> GetCoupon<T>(string couponCode, string token = null)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponAPIBase + "/api/couponAPI/"+couponCode,

                AccessToken = token

            });
        }
    }
}
