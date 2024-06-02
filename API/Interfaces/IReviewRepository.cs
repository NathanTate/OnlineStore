using API.Models.DTO.Feedback;
using FluentResults;
using API.Models.DTO.ProductDTO.Responses;
using API.Models.DTO.ProductDTO.Requests;

namespace API.Interfaces
{
    public interface IReviewRepository
    {
        Task<Result<ReviewResponse>> CreateReviewAsync(ReviewRequest model);
        Task<IEnumerable<ReviewResponse>> GetAllReviewsAsync();
        Task<Result<ReviewResponse>> GetReviewAsync(int id);
        Task<Result> DeleteReviewAsync(int id);
        Task<FeedbackDto> CreateFeedback(FeedbackDto model);
    }
}