using API.Models.DTO.Feedback;
using FluentResults;
using API.Models.DTO.ProductDTO.Responses;
using API.Models.DTO.ProductDTO.Requests;
using API.Helpers;
using API.Helpers.OrderParameters;

namespace API.Interfaces
{
    public interface IReviewRepository
    {
        Task<Result<ReviewResponse>> CreateReviewAsync(ReviewRequest model, string userId);
        Task<PagedList<ReviewResponse>> GetAllReviewsAsync(ReviewParams reviewParams);
        Task<Result<ReviewResponse>> GetReviewAsync(int id);
        Task<Result> DeleteReviewAsync(int id);
        Task<FeedbackDto> CreateFeedbackAsync(FeedbackDto model);
    }
}