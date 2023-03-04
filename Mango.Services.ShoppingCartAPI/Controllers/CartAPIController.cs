using Mango.MessageBus;
using Mango.Services.ShoppingCartAPI.Messeges;
using Mango.Services.ShoppingCartAPI.Model.DTO;
using Mango.Services.ShoppingCartAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartAPIController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        protected ResponseDTO _response;
        private readonly IMessageBus _messageBus;
        public CartAPIController(ICartRepository cartRepository, IMessageBus messageBus)
        {
            _cartRepository= cartRepository;
            _response = new ResponseDTO();
            _messageBus= messageBus;
        }
        [HttpGet("GetCart/{userId}")]
        public async Task<object> GetCart(string userId)
        {
            
            try
            {
                CartDTO cartDtO = await _cartRepository.GetCartByUserIdAsync(userId);
                _response.Result= cartDtO;
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("AddCart")]
        public async Task<object> AddCart([FromBody] CartDTO cartDTO)
        {
           
            try
            {
                CartDTO cartDtO = await _cartRepository.CreateUpdateCartAsync(cartDTO);
                _response.Result = cartDtO;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpPost("UpdateCart")]
        public async Task<object> UpdateCart([FromBody] CartDTO cartDTO)
        {
            try
            {
                CartDTO cartDtO = await _cartRepository.CreateUpdateCartAsync(cartDTO);
                _response.Result = cartDtO;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("RemoveCart")]
        public async Task<object> RemoveCart([FromBody] int cartId)
        {
            try
            {
                var isSuccess = await _cartRepository.RemoveFromCartAsync(cartId);
                _response.Result = isSuccess;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody] CartDTO cartDTO)
        {
            try
            {
                var isSuccess = await _cartRepository.ApplyCoupon(cartDTO.CartHeader.UserId,
                    cartDTO.CartHeader.CouponCode);
                _response.Result = isSuccess;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("RemoveCoupon")]
        public async Task<object> RemoveCoupon([FromBody] string userId)
        {
            try
            {
                var isSuccess = await _cartRepository.RemoveCoupon(userId);
                _response.Result = isSuccess;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        [HttpPost("checkout")]
        public async Task<object> Checkout([FromBody] CheckoutHeaderDTO checkoutHeaderDTO)
        {
           
            try
            {
                CartDTO cartDTO = await _cartRepository.GetCartByUserIdAsync(checkoutHeaderDTO.UserId);
                if (cartDTO == null) return BadRequest();
                checkoutHeaderDTO.CartDetails = cartDTO.CartDetails;
                _messageBus.PublishMessage(checkoutHeaderDTO, "PaymentQueue");
                
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
