using AutoMapper;
using Mango.Services.ShoppingCartAPI.Model;
using Mango.Services.ShoppingCartAPI.Model.DTO;

namespace Mango.Services.ShoppingCartAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mapConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDTO, Product>().ReverseMap();
                config.CreateMap<CartHeader, CartHeaderDTO>().ReverseMap();
                config.CreateMap<CartDetails, CartDetailsDTO>().ReverseMap();
                config.CreateMap<Cart, CartDTO>().ReverseMap();
            });
            return mapConfig;
        }
    }
}
