using API.Models;
using API.Models.DTO.ProductDTO;
using API.Models.DTO.ProductDTO.Requests;
using API.Models.DTO.ProductDTO.Responses;
using API.Models.DTO.UserDTO.Requests;
using API.Models.Product;
using AutoMapper;

namespace API.Helpers
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            return new MapperConfiguration(config =>
            {
                config.CreateMap<RegisterRequest, ApplicationUser>().ReverseMap();
                config.CreateMap<ApplicationUser, UserResponse>();


                config.CreateMap<ProductCategory, ProductCategoryResponse>()
                .ForMember(dest => dest.SubCategoriesDto, opt => opt.MapFrom(src => src.SubCategories));
                config.CreateMap<ProductCategoryRequest, ProductCategory>();
                config.CreateMap<ProductSubCategory, ProductSubCategoryDto>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products))
                .ReverseMap();
                config.CreateMap<ProductItem, ProductItemDto>().ReverseMap();
                config.CreateMap<ProductImage, ProductImageDto>().ReverseMap();
                config.CreateMap<Brand, BrandDto>().ReverseMap();
                config.CreateMap<ProductSpecification, ProductSpecificationDto>().ReverseMap();
                config.CreateMap<ProductRequest, Product>()
                .ForMember(dest => dest.ProductItems, opt => opt.MapFrom(src => src.ProductItemsDto))
                .ForMember(dest => dest.ProductSpecifications, opt => opt.MapFrom(src => src.ProductSpecificationsDto));
                config.CreateMap<Product, ProductResponse>()
                .ForMember(dest => dest.ProductItems, opt => opt.MapFrom(src => src.ProductItems));

                config.CreateMap<Review, ReviewDto>().ReverseMap();
                config.CreateMap<Rating, RatingDto>().ReverseMap();
            });
        }
    }
}
