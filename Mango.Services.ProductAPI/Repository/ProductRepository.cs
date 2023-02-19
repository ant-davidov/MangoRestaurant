using AutoMapper;
using Mango.Services.ProductAPI.DbContexts;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ProductRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;         
        }
        async Task<ProductDTO> IProductRepository.CreateUpdateProduct(ProductDTO productDTO)
        {
            Product product = _mapper.Map<Product>(productDTO);
            if(product.ProductId>0)
            {
                _context.Products.Update(product);
            }
            else
            {
                _context.Products.Add(product);
            }
            await _context.SaveChangesAsync();
            return _mapper.Map<ProductDTO>(product);
        }

        async Task<bool> IProductRepository.DeleteProduct(int id)
        {
            try
            {
                Product product= await _context.Products.FirstOrDefaultAsync(x=>x.ProductId==id);
                if (null == product) return false;
                _context.Products.Remove(product); 
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        async Task<ProductDTO> IProductRepository.GetProductById(int id)
        {
            Product product = await _context.Products.Where(x=> x.ProductId == id).FirstOrDefaultAsync();
            return _mapper.Map<ProductDTO>(product);
        }

        async Task<IEnumerable<ProductDTO>> IProductRepository.GetProducts()
        {
            List<Product> products =  await _context.Products.ToListAsync();
            return _mapper.Map<List<ProductDTO>>(products);
        }
    }
}
