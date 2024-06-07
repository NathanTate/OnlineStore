﻿using API.Helpers;
using API.Helpers.RequestParams;
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
            ModelStateDictionary errors = ValidateModel.Validate(validator, model);

            if (errors.Count > 0) {
                return ValidationProblem(errors);
            }

            var result = await _uow.ProductRepository.CreateProductAsync(model);

            await _uow.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateProduct), new { productId = result.Value.Id }, result.Value);
        }

        [AllowAnonymous]
        [HttpGet("GetProducts")]
        public async Task<ActionResult<PagedList<ProductResponse>>> GetProducts([FromQuery] ProductParams productParams)
        {
            return Ok(await _uow.ProductRepository.GetProductsAsync(productParams));
        }

        [AllowAnonymous]
        [HttpGet("GetProduct/{id}")]
        public async Task<ActionResult<ProductResponse>> GetProduct(int id)
        {
            Result<ProductResponse> result = await _uow.ProductRepository.GetProductAsync(id);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductRequest model, IValidator<ProductRequest> validator)
        {
            ModelStateDictionary errors = ValidateModel.Validate(validator, model);

            if (errors.Count > 0) {
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
        public async Task<ActionResult<ColorDto>> GetColors()
        {
            return Ok(await _uow.ProductRepository.GetColorsAsync());
        }
    }
}
