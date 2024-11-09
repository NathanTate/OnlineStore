using API.Helpers;
using API.Helpers.RequestParams;
using API.Models.DTO.ProductDTO;
using API.Models.DTO.ProductDTO.Requests;
using API.Models.DTO.ProductDTO.Responses;
using FluentResults;

namespace API.Interfaces
{
    public interface IProductRepository
    {
        Task<Result<ProductResponse>> CreateProductPlaceholderAsync();
        Task<PagedList<ProductResponse>> GetProductsAsync(ProductParams productParams);
        Task<Result<ProductResponse>> GetProductAsync(int id);
        Task<Result> UpdateProductAsync(ProductRequest model);
        Task<Result> UpdatePhotosAsync(PhotoUpdateRequest model);
        Task<Result> SetMainPhotoAsync(SetMainPhotoRequest model);
        Task<Result> DeleteProductAsync(int id);
        Task<Result> DeletePhotoAsync(int productId, int photoId);
        Task<IEnumerable<ColorResponse>> GetColorsAsync();
    }
}
