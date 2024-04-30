using API.Models;
using API.Models.DTO.ProductDTO;
using API.Models.DTO.ProductDTO.Requests;
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
                .ForMember(dest => dest.SubCategoriesDto, opt => opt.MapFrom(src => src.SubCategories))
                .ReverseMap();
                config.CreateMap<ProductSubCategory, ProductSubCategoryDto>().ReverseMap();
            });
        }
    }
}
