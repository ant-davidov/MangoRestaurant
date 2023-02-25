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

        public CartController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }
        public async Task<IActionResult> CartIndex()
        {
            return View( await LoadCartDTOBasedOnLoggedInuser());
        }
        public async Task<IActionResult> Remove(int id)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.RemoveFromCartAsync<ResponseDTO>(id, accessToken);
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
                foreach(var item in cartDTO.CartDetails)
                {
                    cartDTO.CartHeader.OrderTotl += (item.Product.Price * item.Count);
                }
            }
            return cartDTO;
        }
    }
}
