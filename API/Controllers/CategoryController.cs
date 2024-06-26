﻿using API.Helpers;
using API.Interfaces;
using API.Models.DTO.ProductDTO;
using API.Models.DTO.ProductDTO.Requests;
using API.Models.DTO.ProductDTO.Responses;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static API.Utility.SD;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(UserRoles.ADMIN))]
    public class CategoryController : BaseAPIController
    {
        private readonly IUnitOfWork _uow;
        public CategoryController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory(ProductCategoryRequest model, [FromServices] IValidator<ProductCategoryRequest> validator)
        {
            ModelStateDictionary errors = ValidateModel.Validate(validator, model);

            if (errors.Count > 0) {
                return ValidationProblem(errors);
            }

            Result<ProductCategoryResponse> result = await _uow.CategoryRepository.CreateCategoryAsync(model);

            if(result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            await _uow.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateCategory), new { categoryId = result.Value.Id}, result.Value);
        }

        [AllowAnonymous]
        [HttpGet("GetCategories")]
        public async Task<ActionResult<IEnumerable<CategoriesResponse>>> GetCategories()
        {
            return Ok(await _uow.CategoryRepository.GetAllCategoryAsync());
        }

        [AllowAnonymous]
        [HttpGet("GetCategory/{id}")]
        public async Task<ActionResult<ProductCategoryResponse>> GetCategory(int id)
        {
            Result<ProductCategoryResponse> result = await _uow.CategoryRepository.GetCategoryAsync(id);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }

        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(ProductCategoryRequest model, IValidator<ProductCategoryRequest> validator)
        {
            ModelStateDictionary errors = ValidateModel.Validate(validator, model);

            if (errors.Count > 0) {
                return ValidationProblem(errors);
            }

            Result result = await _uow.CategoryRepository.UpdateCategoryAsync(model);

            if(result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            await _uow.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            Result result = await _uow.CategoryRepository.DeleteCategoryAsync(id);

            if(result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            await _uow.SaveChangesAsync();

            return Ok();
        }



        [HttpPost("CreateSubCategory")]
        public async Task<IActionResult> CreateSubCategory(ProductSubCategoryDto model, [FromServices] IValidator<ProductSubCategoryDto> validator)
        {
            ModelStateDictionary errors = ValidateModel.Validate(validator, model);

            if (errors.Count > 0) {
                return ValidationProblem(errors);
            }

            Result<ProductSubCategoryDto> result = await _uow.CategoryRepository.CreateSubCategoryAsync(model);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            await _uow.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateSubCategory), new { subCategoryId = result.Value.Id}, result.Value);
        }

        [AllowAnonymous]
        [HttpGet("GetSubCategories/{id}")]
        public async Task<ActionResult<IEnumerable<SubcategoryGroupResponse>>> GetSubCategories(int id = 1)
        {
            return Ok(await _uow.CategoryRepository.GetAllSubCategoryAsync(id));
        }

        [AllowAnonymous]
        [HttpGet("GetSubCategory/{id}")]
        public async Task<ActionResult<ProductSubCategoryDto>> GetSubCategory(int id)
        {
            Result<ProductSubCategoryDto> result = await _uow.CategoryRepository.GetSubCategoryAsync(id);

            if(result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }

        [HttpPut("UpdateSubCategory")]
        public async Task<IActionResult> UpdateSubCategory(ProductSubCategoryDto model, IValidator<ProductSubCategoryDto> validator)
        {
            ModelStateDictionary errors = ValidateModel.Validate(validator, model);

            if (errors.Count > 0) {
                return ValidationProblem(errors);
            }

            Result result = await _uow.CategoryRepository.UpdateSubCategoryAsync(model);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            await _uow.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("DeleteSubCategory/{id}")]
        public async Task<IActionResult> DeleteSubCategory(int id)
        {
            Result result = await _uow.CategoryRepository.DeleteSubCategoryAsync(id);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            await _uow.SaveChangesAsync();

            return Ok();
        }
    }
}
