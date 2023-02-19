using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDTO> list = new ();
            var response = await _productService.GetAllProductsAsync<ResponseDTO>();
            if (response == null || !response.IsSuccess) return new BadRequestResult();
            list = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));        
            return View(list);
        }
        
        public IActionResult ProductCreate() => View();
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductCreate(ProductDTO model)
        {
            if(!ModelState.IsValid) return new BadRequestResult();
            var response = await _productService.CreateProductAsync<ResponseDTO>(model);
            if (response == null || !response.IsSuccess) return new BadRequestResult();
            return RedirectToAction(nameof(ProductIndex));
        }
        [HttpGet]
        public async Task<IActionResult> ProductEdit(int id)
        {
            var response = await _productService.GetAllProductByIdAsync<ResponseDTO>(id);
            if (response == null || !response.IsSuccess) return new BadRequestResult();
            ProductDTO model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));        
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductEdit(ProductDTO model)
        {
            if (!ModelState.IsValid) return new BadRequestResult();
            var response = await _productService.UpdateProductAsync<ResponseDTO>(model);
            if (response == null || !response.IsSuccess) return new BadRequestResult();
            return RedirectToAction(nameof(ProductIndex));
        }

        [HttpGet]
        public async Task<IActionResult> ProductDelete(int id)
        {
            var response = await _productService.GetAllProductByIdAsync<ResponseDTO>(id);
            if (response == null || !response.IsSuccess) return new BadRequestResult();
            ProductDTO model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductDelete(ProductDTO model)
        {
            
            var response = await _productService.DeleteProductAsync<ResponseDTO>(model.ProductId);
            if (response == null || !response.IsSuccess) return new BadRequestResult();
            return RedirectToAction(nameof(ProductIndex));
        }
    }
}
