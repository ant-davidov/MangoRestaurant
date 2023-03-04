using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly ICouponService _couponService;

        public CartController(IProductService productService, ICartService cartService, ICouponService couponService)
        {
            _productService = productService;
            _cartService = cartService;
            _couponService = couponService;
        }
        public async Task<IActionResult> CartIndex()
        {
            return View( await LoadCartDTOBasedOnLoggedInuser());
        }
        public async Task<IActionResult> Checkout()
        {
            return View(await LoadCartDTOBasedOnLoggedInuser());
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(CartDTO cartDTO)
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("acces_token");
                var response = await _cartService.CheckoutAsync<ResponseDTO>(cartDTO.CartHeader, accessToken);
                return RedirectToAction(nameof(Confirmation));
            }
            catch
            {
                return View(cartDTO);
            }
        }

        public async Task<IActionResult> Confirmation()
        {
            return View();
        }

        public async Task<IActionResult> Remove(int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.RemoveFromCartAsync<ResponseDTO>(id, accessToken);
            if (response == null || !response.IsSuccess) return View();
            return RedirectToAction(nameof(CartIndex));
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDTO cartDTO)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.ApplyCouponAsync<ResponseDTO>(cartDTO, accessToken);
            if (response == null || !response.IsSuccess) return View();
            return RedirectToAction(nameof(CartIndex));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDTO cartDTO)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.RemoveCouponAsync<ResponseDTO>(cartDTO.CartHeader.UserId, accessToken);
            if (response == null || !response.IsSuccess) return View();
            return RedirectToAction(nameof(CartIndex));
        }



        private async Task<CartDTO> LoadCartDTOBasedOnLoggedInuser()
        {
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.GetCartByUserIdAsync<ResponseDTO>(userId, accessToken);
            if (response == null || !response.IsSuccess) return new CartDTO();
            var cartDTO = JsonConvert.DeserializeObject<CartDTO>(Convert.ToString(response.Result));
            if(cartDTO.CartHeader != null)
            {
                if (cartDTO.CartHeader != null && !String.IsNullOrEmpty(cartDTO.CartHeader.CouponCode))
                {
                    var coupon = await _couponService.GetCoupon<ResponseDTO>(cartDTO.CartHeader.CouponCode, accessToken);
                    if (coupon != null && coupon.IsSuccess)
                    {
                        var couponObj = JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(coupon.Result));
                        cartDTO.CartHeader.DiscountTotal = couponObj.DiscountAmount;
                    }
                }
                foreach (var item in cartDTO.CartDetails)
                {
                    cartDTO.CartHeader.OrderTotl += (item.Product.Price * item.Count);
                }
               cartDTO.CartHeader.OrderTotl -= cartDTO.CartHeader.DiscountTotal;
            }
            return cartDTO;
        }
    }
}
