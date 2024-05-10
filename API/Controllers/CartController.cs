using API.Extensions;
using API.Interfaces;
using API.Models.DTO.Cart.CartRequests;
using API.Models.DTO.Cart.CartResponses;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPost]
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

            if(result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }

        [HttpPut("AddToCart")]
        public async Task<IActionResult> AddToCart(CartDetailRequest model)
        {
            Result result = await _uow.CartRepository.UpdateCartAsync(model, User.GetUserId());

            if(result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            await _uow.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("RemoveFromCart{cartDetailsId}")]
        public async Task<IActionResult> RemoveFromCart(int cartDetailsId)
        {
            Result result = await _uow.CartRepository.DeleteCartAsync(cartDetailsId, User.GetUserId());

            if(result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            await _uow.SaveChangesAsync();

            return Ok();
        }
    }
}
