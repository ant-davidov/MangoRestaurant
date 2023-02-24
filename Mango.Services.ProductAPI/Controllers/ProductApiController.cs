using Mango.Services.ProductAPI.Models.DTO;
using Mango.Services.ProductAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/products")]
    public class ProductApiController : ControllerBase
    {
        protected ResponseDTO _response;
        private IProductRepository _productRepository;
        public ProductApiController(IProductRepository productRepository)
        {
            
            _productRepository = productRepository;
            this._response = new ResponseDTO();
        }
        
        [HttpGet]
        
        public async Task<object> Get()
        {
            try
            {
                var products = await _productRepository.GetProducts();
                _response.Result = products;
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string> { ex.ToString()};
            }
            return _response;
        }
        
        [HttpGet("{id}")]
        public async Task<object> Get(int id)
        {
            try
            {
                var products = await _productRepository.GetProductById(id);
                _response.Result = products;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string> { ex.ToString() };
            }
            return _response;
        }
        [Authorize]
        [HttpPost]
        public async Task<object> Add([FromBody]ProductDTO productDTO)
        {
            try
            {
                var model = await _productRepository.CreateUpdateProduct(productDTO);
                _response.Result = model;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string> { ex.ToString() };
            }
            return _response;
        }
        [Authorize]
        [HttpPut]
        public async Task<object> Update([FromBody] ProductDTO productDTO)
        {
            try
            {
                var model = await _productRepository.CreateUpdateProduct(productDTO);
                _response.Result = model;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string> { ex.ToString() };
            }
            return _response;
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<object> Delete(int id)
        {
            try
            {
                var res = await _productRepository.DeleteProduct(id);
                _response.Result = res;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string> { ex.ToString() };
            }
            return _response;
        }
    }
}
