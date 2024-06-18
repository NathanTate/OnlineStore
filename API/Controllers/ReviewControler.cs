using API.Extensions;
using API.Helpers;
using API.Helpers.OrderParameters;
using API.Interfaces;
using API.Models.DTO.Feedback;
using API.Models.DTO.ProductDTO.Requests;
using API.Models.DTO.ProductDTO.Responses;
using FluentResults;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ReviewController : BaseAPIController
    {
        private readonly IUnitOfWork _uow;
        public ReviewController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost("CreateReview")]
        public async Task<ActionResult<ReviewResponse>> CreateReview(ReviewRequest model, IValidator<ReviewRequest> validator)
        {
            ModelStateDictionary errors = ValidateModel.Validate(validator, model);

            if (errors.Count > 0)
            {
                return ValidationProblem(errors);
            }

            Result<ReviewResponse> review = await _uow.ReviewRepository.CreateReviewAsync(model, User.GetUserId());

            if (review.IsFailed)
            {
                return BadRequest(review.Errors);
            }

            await _uow.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateReview), new { reviewId = review.Value.Id }, review.Value);
        }

        [AllowAnonymous]
        [HttpGet("GetReviews")]
        public async Task<ActionResult<PagedList<ReviewResponse>>> GetReviews([FromQuery] ReviewParams reviewParams)
        {
            return Ok(await _uow.ReviewRepository.GetAllReviewsAsync(reviewParams));
        }

        [HttpGet("GetReview/{id}")]
        public async Task<ActionResult<ReviewResponse>> GetReview(int id)
        {
            Result<ReviewResponse> result = await _uow.ReviewRepository.GetReviewAsync(id);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }

        [HttpDelete("DeleteReview/{id}")]
        public async Task<IActionResult> DeleteReview(int id) 
        {
            Result result = await _uow.ReviewRepository.DeleteReviewAsync(id);

            if(result.IsFailed) 
            {
                return BadRequest(result.Errors);
            }

            await _uow.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("CreateFeedback")]
        public async Task<ActionResult<FeedbackDto>> CreateFeedback(FeedbackDto model, IValidator<FeedbackDto> validator)
        {
            ModelStateDictionary errors = ValidateModel.Validate(validator, model);

            if (errors.Count > 0)
            {
                return ValidationProblem(errors);
            }

            var feedback = await _uow.ReviewRepository.CreateFeedbackAsync(model);
            await _uow.SaveChangesAsync();

            return Ok(feedback);
        }

    }
}
