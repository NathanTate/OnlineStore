using API.Extensions;
using API.Helpers.OrderParameters;
using API.Interfaces;
using API.Models.DTO.Cart.CartResponses;
using API.Models.DTO.Order;
using API.Models.DTO.Order.Requests;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static API.Utility.SD;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class OrderController : BaseAPIController
    {
        private readonly IUnitOfWork _uow;
        public OrderController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost("Checkout")]
        public async Task<IActionResult> Checkout(OrderCheckoutRequest model)
        {
            Result<string> result = await _uow.OrderRepository.CheckoutAsync(model, User.GetUserId());

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            await _uow.SaveChangesAsync();

            Response.Headers.Append("Access-Control-Expose-Headers", "Location");
            Response.Headers.Location = result.Value;
            return Ok();
        }

        [HttpPost("ValidateStripeSession{id}")]
        public async Task<IActionResult> ValidateStripeSession(int id)
        {
            Result result = await _uow.OrderRepository.VerifyStripeSessionAsync(id);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            await _uow.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("GetOrders")]
        public async Task<ActionResult<IEnumerable<OrderHeaderDto>>> GetOrders([FromQuery] OrderParams orderParams)
        {
            return Ok(await _uow.OrderRepository.GetOrdersAsync(orderParams, User.GetUserId()));
        }

        [HttpGet("GetOrder{id}")]
        public async Task<ActionResult<OrderHeaderDto>> GetOrder(int id)
        {
            Result<OrderHeaderDto> result = await _uow.OrderRepository.GetOrderAsync(id, User.GetUserId());

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(UserRoles.ADMIN))]
        [HttpPut("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(OrderUpdateRequest model)
        {
            Result result = await _uow.OrderRepository.UpdateOrderAsync(model);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            await _uow.SaveChangesAsync();

            return Ok();
        }
    }
}
