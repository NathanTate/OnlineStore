using API.Models.DTO.ProductDTO;
using API.Models.DTO.ProductDTO.Requests;
using API.Models.DTO.ProductDTO.Responses;
using FluentResults;

namespace API.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Result<ProductCategoryResponse>> CreateCategoryAsync(ProductCategoryRequest model);
        Task<IEnumerable<CategoriesResponse>> GetAllCategoryAsync();
        Task<Result<ProductCategoryResponse>> GetCategoryAsync(int id);
        Task<Result> UpdateCategoryAsync(ProductCategoryRequest model);
        Task<Result> DeleteCategoryAsync(int id);


        Task<Result<ProductSubCategoryDto>> CreateSubCategoryAsync(ProductSubCategoryDto model);
        Task<IEnumerable<SubcategoryGroupResponse>> GetAllSubCategoryAsync(int id = 1);
        Task<Result<ProductSubCategoryDto>> GetSubCategoryAsync(int id);
        Task<Result> UpdateSubCategoryAsync(ProductSubCategoryDto model);
        Task<Result> DeleteSubCategoryAsync(int id);
    }
}
