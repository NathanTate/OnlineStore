using API.Helpers.RequestParams;
using API.Interfaces;
using API.Models.DTO.ProductDTO.Requests;
using API.Models.DTO.ProductDTO.Responses;
using API.Models.Product;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Controllers
{
    public class ProductController : BaseAPIController
    {
        private readonly IUnitOfWork _uow;
        public ProductController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost("CreateProduct")]
        public async Task<ActionResult<ProductResponse>> CreateProduct([FromForm] ProductRequest model, IValidator<ProductRequest> validator)
        {
            ValidationResult validationResult = validator.Validate(model);

            if (!validationResult.IsValid) 
            {
                var modelStateDictionary = new ModelStateDictionary();
                foreach(ValidationFailure failure in validationResult.Errors)
                {
                    modelStateDictionary.AddModelError(
                        failure.PropertyName,
                        failure.ErrorMessage);
                }

                return ValidationProblem(modelStateDictionary);
            }

            var result = await _uow.ProductRepository.CreateProductAsync(model);

            await _uow.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateProduct), new {productId = result.Value.Id }, result.Value);
        }

        [HttpGet("GetProducts")]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProducts([FromQuery] ProductParams productParams)
        {
            return Ok(await _uow.ProductRepository.GetProductsAsync(productParams));
        }

        [HttpGet("GetProduct{id}")]
        public async Task<ActionResult<ProductResponse>> GetProduct(int id)
        {
            Result<ProductResponse> result = await _uow.ProductRepository.GetProductAsync(id);

            if(result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductRequest model, IValidator<ProductRequest> validator)
        {
            ValidationResult validationResult = validator.Validate(model);

            if(!validationResult.IsValid) 
            {
                var modelStateDictionary = new ModelStateDictionary();
                foreach(ValidationFailure failure in validationResult.Errors)
                {
                    modelStateDictionary.AddModelError(
                        failure.PropertyName,
                        failure.ErrorMessage);
                }

                return ValidationProblem(modelStateDictionary);
            }

            Result result = await _uow.ProductRepository.UpdateProductAsync(model);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            await _uow.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("DeleteProduct{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            Result result = await _uow.ProductRepository.DeleteProductAsync(id);

            if(result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            await _uow.SaveChangesAsync();

            return Ok();
        }
    }
}
