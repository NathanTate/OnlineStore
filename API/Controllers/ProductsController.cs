using API.Interfaces;
using API.Models.DTO.ProductDTO;
using API.Models.DTO.ProductDTO.Requests;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Controllers
{
    public class ProductsController : BaseAPIController
    {
        private readonly IUnitOfWork _uow;
        public ProductsController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory(ProductCategoryResponse model, [FromServices] IValidator<ProductCategoryResponse> validator)
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

            await _uow.ProductRepository.CreateCategory(model);

            await _uow.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("GetCategories")]
        public async Task<ActionResult<IEnumerable<ProductCategoryResponse>>> GetCategories()
        {
            var categories = await _uow.ProductRepository.GetAllCategory();

            return Ok(categories);
        }
        [HttpPost("CreateSubCategory")]
        public async Task<IActionResult> CreateSubCategory(ProductSubCategoryDto model, [FromServices] IValidator<ProductSubCategoryDto> validator)
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

            await _uow.ProductRepository.CreateSubCategory(model);

            await _uow.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("GetCtegory{id}")]
        public async Task<ActionResult<ProductCategoryResponse>> GetCategory(int id)
        {
            return Ok(await _uow.ProductRepository.GetCategory(id));
        }
    }
}
