using API.Helpers;
using API.Interfaces;
using API.Models.DTO.Coupon;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Stripe;
using static API.Utility.SD;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(UserRoles.ADMIN))]
    public class CouponController : BaseAPIController
    {
        private readonly IUnitOfWork _uow;
        public CouponController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost("CreateCoupon")]
        public async Task<ActionResult<CouponDto>> CreateCoupon([FromBody] CouponDto model, [FromServices] IValidator<CouponDto> validator)
        {
            ModelStateDictionary errors = ValidateModel.Validate(validator, model);

            if (errors.Count > 0) {
                return ValidationProblem(errors);
            }

            Result<CouponDto> result = await _uow.CouponRepository.CreateCouponAsync(model);

            if(result.IsFailed) 
            {
                return BadRequest(result.Errors);
            }

            await _uow.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateCoupon), result.Value);
        }

        [HttpGet("GetCoupons")]
        public async Task<ActionResult<IEnumerable<CouponDto>>> GetCoupons()
        {
            return Ok(await _uow.CouponRepository.GetCouponsAsync());
        }

        [HttpGet("GetCoupon{code}")]
        public async Task<ActionResult<CouponDto>> GetCoupon(string code)
        {
            Result<CouponDto> result = await _uow.CouponRepository.GetCouponAsync(code);

            if(result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Value);
        }

        [HttpPut("UpdateCoupon")]
        public async Task<IActionResult> UpdateCoupon([FromBody] CouponDto model, [FromServices] IValidator<CouponDto> validator)
        {
            ModelStateDictionary errors = ValidateModel.Validate(validator, model);

            if (errors.Count > 0) {
                return ValidationProblem(errors);
            }

            Result result = await _uow.CouponRepository.UpdateCouponAsync(model);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            await _uow.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCoupon(string code)
        {
            Result result = await _uow.CouponRepository.DeleteCouponAsync(code);

            if(result.IsFailed)
            {
                return BadRequest(result.Errors);   
            }

            await _uow.SaveChangesAsync();

            return Ok();
        }
    }
}
