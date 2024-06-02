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

        public async Task<Result<ReviewResponse>> CreateReviewAsync(ReviewRequest model)
        {
            var review = _mapper.Map<Review>(model);

            await _dbContext.Reviews.AddAsync(review);

            return Result.Ok(_mapper.Map<ReviewResponse>(review));
        }

        public async Task<IEnumerable<ReviewResponse>> GetAllReviewsAsync()
        {
            return  await _dbContext.Reviews
                .ProjectTo<ReviewResponse>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Result<ReviewResponse>> GetReviewAsync(int id)
        {
            var review = await _dbContext.Reviews.FindAsync(id);

            if(review == null)
            {
                return Result.Fail("Review doesn't exist");
            }

            await _dbContext.Entry(review).Reference(r => r.Rating).LoadAsync();

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

        public async Task<FeedbackDto> CreateFeedback(FeedbackDto model) 
        {
            var feedback = _mapper.Map<Feedback>(model);

            await _dbContext.Feedbacks.AddAsync(feedback);

            return _mapper.Map<FeedbackDto>(feedback);
        }
    }
}
