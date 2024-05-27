using API.Models;
using API.Models.Cart;
using API.Models.Coupon;
using API.Models.DTO.Address.Requests;
using API.Models.DTO.Cart.CartRequests;
using API.Models.DTO.Cart.CartResponses;
using API.Models.DTO.Coupon;
using API.Models.DTO.Order;
using API.Models.DTO.Order.Requests;
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
                config.CreateMap<AddressRequest, Address>();


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
                .ForMember(dest => dest.ProductSpecifications, opt => opt.MapFrom(src => src.ProductSpecifications))
                .ForMember(dest => dest.ProductImages, opt => opt.Ignore())
                .ForMember(dest => dest.Colors, opt => opt.Ignore());
                config.CreateMap<Product, ProductResponse>();

                config.CreateMap<Review, ReviewDto>().ReverseMap();
                config.CreateMap<Rating, RatingDto>().ReverseMap();

                config.CreateMap<Coupon, CouponDto>().ReverseMap();

                config.CreateMap<CartHeader, CartHeaderRequest>().ReverseMap();
                config.CreateMap<CartHeader, CartHeaderResponse>().ReverseMap();
                config.CreateMap<CartDetail, CartDetailRequest>().ReverseMap();
                config.CreateMap<CartDetail, CartDetailResponse>().ReverseMap();

                config.CreateMap<OrderCheckoutRequest, OrderHeaderDto>()
                .ForMember(dest => dest.OrderTotal, opt => opt.MapFrom(src => src.CartResponse.CartHeader.Total))
                .ForMember(dest => dest.CouponCode, opt => opt.MapFrom(src => src.CartResponse.CartHeader.CouponCode))
                .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.CartResponse.CartHeader.Discount)).ReverseMap();
                config.CreateMap<CartDetailResponse, OrderDetailDto>()
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.OriginalPrice))
                .ReverseMap();
                config.CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
                config.CreateMap<OrderDetail, OrderDetailDto>().ReverseMap();

                config.CreateMap<Color, ColorDto>().ReverseMap();
                config.CreateMap<ProductColor, ColorDto>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Color.Value))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Color.Id)).ReverseMap();
            });
        }
    }
}
