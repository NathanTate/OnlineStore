using API.Interfaces;
using API.Models.DTO.ProductDTO;
using API.Models.Product;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentResults;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Result<ReviewDto>> CreateReviewAsync(ReviewDto model)
        {
            var review = _mapper.Map<Review>(model);

            await _dbContext.Reviews.AddAsync(review);

            return Result.Ok(_mapper.Map<ReviewDto>(review));
        }

        public async Task<IEnumerable<ReviewDto>> GetAllReviewsAsync()
        {
            return  await _dbContext.Reviews
                .ProjectTo<ReviewDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Result<ReviewDto>> GetReviewAsync(int id)
        {
            var review = await _dbContext.Reviews.FindAsync(id);

            if(review == null)
            {
                return Result.Fail("Review doesn't exist");
            }

            await _dbContext.Entry(review).Reference(r => r.Rating).LoadAsync();

            return _mapper.Map<ReviewDto>(review);
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
    }
}
