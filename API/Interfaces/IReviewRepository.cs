using API.Models.DTO.ProductDTO;
using FluentResults;

namespace API.Interfaces
{
    public interface IReviewRepository
    {
        Task<Result<ReviewDto>> CreateReviewAsync(ReviewDto model);
        Task<IEnumerable<ReviewDto>> GetAllReviewsAsync();
        Task<Result<ReviewDto>> GetReviewAsync(int id);
        Task<Result> DeleteReviewAsync(int id);
    }
}