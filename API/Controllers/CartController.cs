﻿using API.Extensions;
using API.Helpers;
using API.Interfaces;
using API.Models.DTO.Cart.CartRequests;
using API.Models.DTO.Cart.CartResponses;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CartController : BaseAPIController
    {
        private readonly IUnitOfWork _uow;
        public CartController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost("CreateCart")]
        public async Task<ActionResult<CartResponse>> CreateCart()
        {
            Result<CartResponse> result = await _uow.CartRepository.CreateCartAsync(User.GetUserId());

            if(result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            await _uow.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateCart), result.Value);
        }

        [HttpGet("GetCart")]
        public async Task<ActionResult<CartResponse>> GetCart()
        {
            Result<CartResponse> result = await _uow.CartRepository.GetCartAsync(User.GetUserId());

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }

        [HttpGet("CartExists")]
        public async Task<ActionResult<bool>> CartExists()
        {
            return Ok(await _uow.CartRepository.CartExistsAsync(User.GetUserId()));
        }

        [HttpPost("ApplyCoupon")]
        public async Task<IActionResult> ApplyCoupon(CartHeaderRequest model, [FromServices] IValidator<CartHeaderRequest> validator)
        {
            ModelStateDictionary errors = ValidateModel.Validate(validator, model);

            if (errors.Count > 0) {
                return ValidationProblem(errors);
            }

            Result result = await _uow.CartRepository.ApplyCouponAsync(model, User.GetUserId());

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            await _uow.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("UpdateCart")]
        public async Task<IActionResult> UpdateCart(CartDetailRequest model, [FromServices] IValidator<CartDetailRequest> validator)
        {
           ModelStateDictionary errors = ValidateModel.Validate(validator, model);

            if (errors.Count > 0) {
                return ValidationProblem(errors);
            }

            Result result = await _uow.CartRepository.UpdateCartAsync(model, User.GetUserId());

            if(result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            await _uow.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("RemoveFromCart/{cartDetailsId}/{removeAll}")]
        public async Task<IActionResult> RemoveFromCart(int cartDetailsId, bool removeAll)
        {
            Result result = await _uow.CartRepository.RemoveFromCartAsync(cartDetailsId, removeAll, User.GetUserId());

            if(result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            await _uow.SaveChangesAsync();

            return Ok();
        }
    }
}
