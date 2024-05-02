using API.Models.DTO.ProductDTO;
using API.Models.DTO.ProductDTO.Requests;
using FluentResults;

namespace API.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Result<ProductCategoryResponse>> CreateCategoryAsync(ProductCategoryRequest model);
        Task<IEnumerable<ProductCategoryResponse>> GetAllCategoryAsync();
        Task<Result<ProductCategoryResponse>> GetCategoryAsync(int id);
        Task<Result> UpdateCategoryAsync(ProductCategoryRequest model);
        Task<Result> DeleteCategoryAsync(int id);


        Task<Result<ProductSubCategoryDto>> CreateSubCategoryAsync(ProductSubCategoryDto model);
        Task<IEnumerable<ProductSubCategoryDto>> GetAllSubCategoryAsync(int id = 1);
        Task<Result<ProductSubCategoryDto>> GetSubCategoryAsync(int id);
        Task<Result> UpdateSubCategoryAsync(ProductSubCategoryDto model);
        Task<Result> DeleteSubCategoryAsync(int id);
    }
}
