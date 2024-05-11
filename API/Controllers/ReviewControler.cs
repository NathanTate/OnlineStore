using API.Interfaces;
using API.Models.DTO.ProductDTO;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ReviewControler : BaseAPIController
    {
        private readonly IUnitOfWork _uow;
        public ReviewControler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost("CreateReview")]
        public async Task<ActionResult<ReviewDto>> CreateReview(ReviewDto model, IValidator<ReviewDto> validator)
        {
            ValidationResult validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                var modelStateDictionary = new ModelStateDictionary();
                foreach (ValidationFailure failure in validationResult.Errors)
                {
                    modelStateDictionary.AddModelError(
                        failure.PropertyName,
                        failure.ErrorMessage);
                }

                return ValidationProblem(modelStateDictionary);
            }

            Result<ReviewDto> review = await _uow.ReviewRepository.CreateReviewAsync(model);

            if(review.IsFailed)
            {
                return BadRequest(review.Errors);
            }

            await _uow.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateReview), new {reviewId = review.Value.Id}, review.Value);
        }

        [AllowAnonymous]
        [HttpGet("GetReviews")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviews()
        {
            return Ok(await _uow.ReviewRepository.GetAllReviewsAsync());
        }

        [HttpGet("GetReview{id}")]
        public async Task<ActionResult<ReviewDto>> GetReview(int id)
        {
            Result<ReviewDto> result = await _uow.ReviewRepository.GetReviewAsync(id);

            if(result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }

    }
}
