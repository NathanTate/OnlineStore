using API.Extensions;
using API.Helpers;
using API.Helpers.OrderParameters;
using API.Interfaces;
using API.Models.DTO.Order;
using API.Models.DTO.Order.Requests;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        public async Task<IActionResult> Checkout(OrderCheckoutRequest model, [FromServices]IValidator<OrderCheckoutRequest> validator)
        {
            ModelStateDictionary errors = ValidateModel.Validate(validator, model);

            if (errors.Count > 0) {
                return ValidationProblem(errors);
            }

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

        [HttpPost("ValidateStripeSession/{id}")]
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
        public async Task<ActionResult<PagedList<OrderHeaderDto>>> GetOrders([FromQuery] OrderParams orderParams)
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

        [Authorize(Roles = nameof(UserRoles.ADMIN))]
        [HttpPut("UpdateStatus")]
        public async Task<IActionResult> UpdateOrder(OrderUpdateRequest model, [FromServices]IValidator<OrderUpdateRequest> validator)
        {
            ModelStateDictionary errors = ValidateModel.Validate(validator, model);

            if (errors.Count > 0) {
                return ValidationProblem(errors);
            }

            Result result = await _uow.OrderRepository.UpdateOrderAsync(model);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            await _uow.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = nameof(UserRoles.ADMIN))]
        [HttpDelete("DeleteOrder/{id}")]
        public async Task<IActionResult> DeleteOrder(int id) 
        {
            Result result = await _uow.OrderRepository.DeleteOrderAsync(id);

            if(result.IsFailed) {
                return BadRequest(result.Errors);
            }

            await _uow.SaveChangesAsync();

            return Ok();
        }
    }
}
