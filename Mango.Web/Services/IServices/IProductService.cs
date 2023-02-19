using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mango.Web.Models;

namespace Mango.Web.Services.IServices
{
    public interface IProductService
    {
        Task<T> GetAllProductsAsync<T>();
        Task<T> GetAllProductByIdAsync<T>(int id);
        Task<T> CreateProductAsync<T>(ProductDTO productDTO);
        Task<T> UpdateProductAsync<T>(ProductDTO productDTO);
        Task<T> DeleteProductAsync<T>(int id);
    }
}