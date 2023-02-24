namespace TP.NA.CartService.Application.Config
{
    using AutoMapper;
    using CartService.Application.Models;
    using CartService.Domain.Entities;

    /// <summary>
    /// Mapper configuration
    /// </summary>
    public class AutoMapperConfig : Profile
    {
        /// <summary>
        /// Create a mapper class
        /// </summary>
        public AutoMapperConfig()
        {
            CreateMap<ProductEntity, ProductModel>().ReverseMap();
            CreateMap<CartEntity, CartModel>().ReverseMap();
        }
    }
}