using API.Helpers;
using API.Helpers.RequestParams;
using API.Interfaces;
using API.Models.DTO.ProductDTO;
using API.Models.DTO.ProductDTO.Requests;
using API.Models.DTO.ProductDTO.Responses;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Memory;
using static API.Utility.SD;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(UserRoles.ADMIN))]
    public class ProductController : BaseAPIController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMemoryCache _memoryCache;
        public ProductController(IUnitOfWork uow, IMemoryCache memoryCache)
        {
            _uow = uow;
            _memoryCache = memoryCache;
        }

        [HttpPost("CreatePlaceholder")]
        public async Task<ActionResult<ProductResponse>> CreatePlaceholder()
        {
            Result<ProductResponse> result = await _uow.ProductRepository.CreateProductPlaceholderAsync();

            await _uow.SaveChangesAsync();

            return CreatedAtAction(nameof(CreatePlaceholder), new { productId = result.Value.Id }, result.Value);
        }

        [AllowAnonymous]
        [HttpGet("GetProducts")]
        public async Task<ActionResult<PagedList<ProductResponse>>> GetProducts([FromQuery] ProductParams productParams)
        {
            var productResponse = await _memoryCache.GetOrCreateAsync(productParams.ToString(), async entry =>
            {
                var productResponse = await _uow.ProductRepository.GetProductsAsync(productParams);
                foreach (var product in productResponse.Items)
                {
                    if (!product.Reviews.Any())
                    {
                        continue;
                    }

                    double totalRating = 0;
                    foreach (var rating in product.Reviews)
                    {
                        totalRating += rating.RatingScore;
                    }
                    product.ProductRating = Math.Round(totalRating / product.Reviews.Count(), 2);
                    product.TotalReviews = product.Reviews.Count();

                }
                entry.SlidingExpiration = TimeSpan.FromSeconds(30);
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2);
                return productResponse;
            });
            return Ok(productResponse);
        }

        [AllowAnonymous]
        [HttpGet("GetProduct/{id}")]
        public async Task<ActionResult<ProductResponse>> GetProduct(int id)
        {
            Result<ProductResponse> result = await _uow.ProductRepository.GetProductAsync(id);

            if (result.IsFailed)
            {
                return NotFound(result.Errors);
            }

            if (result.Value.Reviews.Any())
            {
                double totalRating = 0;
                foreach (var rating in result.Value.Reviews)
                {
                    totalRating += rating.RatingScore;
                }
                result.Value.ProductRating = Math.Round(totalRating / result.Value.Reviews.Count(), 2);
                result.Value.TotalReviews = result.Value.Reviews.Count();
            }

            return Ok(result.Value);
        }

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductRequest model, IValidator<ProductRequest> validator)
        {
            ModelStateDictionary errors = ValidateModel.Validate(validator, model);

            if (errors.Count > 0)
            {
                return ValidationProblem(errors);
            }

            Result result = await _uow.ProductRepository.UpdateProductAsync(model);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            await _uow.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("UpdatePhotos")]
        public async Task<IActionResult> UpdatePhotos([FromForm] PhotoUpdateRequest model, IValidator<PhotoUpdateRequest> validator)
        {
            ModelStateDictionary errors = ValidateModel.Validate(validator, model);

            if (errors.Count > 0)
            {
                return ValidationProblem(errors);
            }

            Result result = await _uow.ProductRepository.UpdatePhotosAsync(model);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            await _uow.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("SetMainPhoto")]
        public async Task<IActionResult> SetMainPhoto(SetMainPhotoRequest model, IValidator<SetMainPhotoRequest> validator)
        {
            ModelStateDictionary errors = ValidateModel.Validate(validator, model);

            if (errors.Count > 0)
            {
                return ValidationProblem(errors);
            }

            Result result = await _uow.ProductRepository.SetMainPhotoAsync(model);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors[0]);
            }

            await _uow.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            Result result = await _uow.ProductRepository.DeleteProductAsync(id);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            await _uow.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("DeletePhoto/{productId}")]
        public async Task<IActionResult> DeletePhoto(int productId, [FromQuery] int photoId)
        {
            Result result = await _uow.ProductRepository.DeletePhotoAsync(productId, photoId);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            await _uow.SaveChangesAsync();

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("GetColors")]
        public async Task<ActionResult<ColorResponse>> GetColors()
        {
            return Ok(await _uow.ProductRepository.GetColorsAsync());
        }
    }
}
