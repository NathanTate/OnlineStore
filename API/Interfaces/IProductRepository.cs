using API.Helpers;
using API.Helpers.RequestParams;
using API.Models.DTO.ProductDTO.Requests;
using API.Models.DTO.ProductDTO.Responses;
using FluentResults;

namespace API.Interfaces
{
    public interface IProductRepository
    {
        Task<Result<ProductResponse>> CreateProductAsync(ProductRequest model);
        Task<PagedList<ProductResponse>> GetProductsAsync(ProductParams productParams);
        Task<Result<ProductResponse>> GetProductAsync(int id);
        Task<Result> UpdateProductAsync(ProductRequest model);
        Task<Result> DeleteProductAsync(int id);
        Task<Result> DeletePhotoAsync(int productId, int photoId);
    }
}
