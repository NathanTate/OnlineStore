using API.Models.DTO.ProductDTO;
using API.Models.DTO.ProductDTO.Requests;
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    public interface IProductRepository
    {
        Task<ProductCategoryResponse> CreateCategory(ProductCategoryResponse model);
        Task<ProductSubCategoryDto> CreateSubCategory(ProductSubCategoryDto model);

        Task<IEnumerable<ProductCategoryResponse>> GetAllCategory();
        Task<ProductCategoryResponse> GetCategory(int categoryId);
        Task<ProductSubCategoryDto> GetAllSubCategory();
    }
}
