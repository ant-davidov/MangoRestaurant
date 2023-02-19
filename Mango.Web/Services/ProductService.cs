using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mango.Web.Models;
using Mango.Web.Services.IServices;

namespace Mango.Web.Services
{
    public class ProductService : BaseService, IProductService
    {   
        public ProductService(IHttpClientFactory httpClient) : base(httpClient) {}
        
        

        public async Task<T> CreateProductAsync<T>(ProductDTO productDTO)
        {
           return await SendAsync<T> (new ApiRequest()
           {
             ApiType = SD.ApiType.POST,
             Data = productDTO,
             Url = SD.ProductAPIBase + "/api/products",
             AccessToken =String.Empty
           });
        }

        public async Task<T> DeleteProductAsync<T>(int id)
        {
            return await SendAsync<T> (new ApiRequest()
           {
             ApiType = SD.ApiType.DELETE,
             Url = SD.ProductAPIBase + "/api/products/"+id,
             AccessToken =String.Empty
           });
        }

        public async Task<T> GetAllProductByIdAsync<T>(int id)
        {
              return await SendAsync<T> (new ApiRequest()
           {
             ApiType = SD.ApiType.GET,
             Url = SD.ProductAPIBase + "/api/products/"+id,
             AccessToken =String.Empty

           });
        }

        public async Task<T> GetAllProductsAsync<T>()
        {
            return await SendAsync<T> (new ApiRequest()
           {
             ApiType = SD.ApiType.GET,
             Url = SD.ProductAPIBase + "/api/products",
             AccessToken =String.Empty

           });
        }

        public async Task<T> UpdateProductAsync<T>(ProductDTO productDTO)
        {
           return await SendAsync<T> (new ApiRequest()
           {
             ApiType = SD.ApiType.PUT,
             Data = productDTO,
             Url = SD.ProductAPIBase + "/api/products",
             AccessToken =String.Empty
           });
        }
    }
}