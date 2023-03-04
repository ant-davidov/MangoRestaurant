using Mango.Services.CouponAPI.Model.DTO;
using Mango.Services.CouponAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        protected ResponseDTO _response;
        private ICouponRepository _productRepository;
        public CouponAPIController(ICouponRepository productRepository)
        {

            _productRepository = productRepository;
            _response = new ResponseDTO();
        }

        [HttpGet("{code}")]
        public async Task<object> GetDiscountForCode(string code)
        {

            try
            {
                var coupon = await _productRepository.GetCouponByCode(code);
                _response.Result = coupon;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}
