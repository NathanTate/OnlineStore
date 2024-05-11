using API.Models;
using API.Models.Cart;
using API.Models.Coupon;
using API.Models.DTO.Cart.CartRequests;
using API.Models.DTO.Cart.CartResponses;
using API.Models.DTO.Coupon;
using API.Models.DTO.Order;
using API.Models.DTO.ProductDTO;
using API.Models.DTO.ProductDTO.Requests;
using API.Models.DTO.ProductDTO.Responses;
using API.Models.DTO.UserDTO.Requests;
using API.Models.Order;
using API.Models.ProductModel;
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

                config.CreateMap<ProductImage, ProductImageDto>().ReverseMap();
                config.CreateMap<Brand, BrandDto>().ReverseMap();
                config.CreateMap<ProductSpecification, ProductSpecificationDto>().ReverseMap();
                config.CreateMap<ProductRequest, Product>()
                .ForMember(dest => dest.ProductSpecifications, opt => opt.MapFrom(src => src.ProductSpecificationsDto));
                config.CreateMap<Product, ProductResponse>();

                config.CreateMap<Review, ReviewDto>().ReverseMap();
                config.CreateMap<Rating, RatingDto>().ReverseMap();

                config.CreateMap<Coupon, CouponDto>().ReverseMap();

                config.CreateMap<CartHeader, CartHeaderRequest>().ReverseMap();
                config.CreateMap<CartHeader, CartHeaderResponse>().ReverseMap();
                config.CreateMap<CartDetail, CartDetailRequest>().ReverseMap();
                config.CreateMap<CartDetail, CartDetailResponse>().ReverseMap();

                config.CreateMap<OrderHeaderDto, CartHeaderResponse>().ReverseMap();
                config.CreateMap<CartDetailResponse, OrderDetailDto>()
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.OriginalPrice))
                .ReverseMap();
                config.CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
                config.CreateMap<OrderDetail, OrderDetailDto>().ReverseMap();
            });
        }
    }
}
