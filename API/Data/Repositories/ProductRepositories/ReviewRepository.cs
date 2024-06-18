using API.Interfaces;
using API.Models;
using API.Models.DTO.Feedback;
using API.Models.ProductModel;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using API.Models.DTO.ProductDTO.Responses;
using API.Models.DTO.ProductDTO.Requests;
using API.Helpers;
using API.Helpers.OrderParameters;
using System.Linq.Expressions;
using static API.Utility.SD;

namespace API.Data.Repositories.ProductRepositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public ReviewRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Result<ReviewResponse>> CreateReviewAsync(ReviewRequest model, string userId)
        {
            var review = _mapper.Map<Review>(model);
            if(string.IsNullOrEmpty(review.Comment)) review.Comment = "";
            if(string.IsNullOrEmpty(review.OrderStatus)) review.OrderStatus = "UNKNOWN";
            review.CreatedAt = DateTime.UtcNow;
            review.UserId = userId;

            await _dbContext.Reviews.AddAsync(review);

            return Result.Ok(_mapper.Map<ReviewResponse>(review));
        }

        public async Task<PagedList<ReviewResponse>> GetAllReviewsAsync(ReviewParams reviewParams)
        {
            IQueryable<Review> reviewsQuery = _dbContext.Reviews.AsQueryable()
                .Where(r => r.ProductId == reviewParams.Id);

            reviewsQuery = FilterReviews(reviewsQuery, reviewParams);

            if (reviewParams.SortBy?.ToLower() == "desc") 
            {
                reviewsQuery = reviewsQuery.OrderByDescending(GetSortProperty(reviewParams));
            }
            else
            {
                reviewsQuery = reviewsQuery.OrderBy(GetSortProperty(reviewParams));
            }

            var result = await PagedList<ReviewResponse>.CreateAsync(
                reviewsQuery.AsNoTracking().ProjectTo<ReviewResponse>(_mapper.ConfigurationProvider),
                reviewParams.Page,
                reviewParams.PageSize);

            return result;
        }

        public async Task<Result<ReviewResponse>> GetReviewAsync(int id)
        {
            var review = await _dbContext.Reviews.FindAsync(id);

            if (review == null)
            {
                return Result.Fail("Review doesn't exist");
            }

            return _mapper.Map<ReviewResponse>(review);
        }

        public async Task<Result> DeleteReviewAsync(int id)
        {
            var review = await _dbContext.Reviews.FindAsync(id);

            if (review == null)
            {
                return Result.Fail("Review doesn't exist");
            }

            _dbContext.Reviews.Remove(review);

            return Result.Ok();
        }

        public async Task<FeedbackDto> CreateFeedbackAsync(FeedbackDto model)
        {
            var feedback = _mapper.Map<Feedback>(model);

            await _dbContext.Feedbacks.AddAsync(feedback);

            return _mapper.Map<FeedbackDto>(feedback);
        }

        private static Expression<Func<Review, object>> GetSortProperty(ReviewParams sortColumn)
        {
            Expression<Func<Review, object>> keySelector = sortColumn.SortColumn?.ToLower() switch
            {
                "date" => review => review.CreatedAt,
                _ => review => review.Id
            };

            return keySelector;
        }

        private static IQueryable<Review> FilterReviews(IQueryable<Review> reviewsQuery, ReviewParams reviewParams)
        {
            if(reviewParams.Stars != default(int)) 
            {
                reviewsQuery = reviewsQuery.Where(r => r.RatingScore == reviewParams.Stars * 10);
            }
            if(reviewParams.SortColumn == "bought") {
                reviewsQuery = reviewsQuery
                .Where(r => r.OrderStatus == nameof(OrderStatus.COMPLETED) || 
                r.OrderStatus == nameof(OrderStatus.APPROVED) ||
                r.OrderStatus == nameof(OrderStatus.READYTOSHIP));
            }

            return reviewsQuery;
        }
    }
}
