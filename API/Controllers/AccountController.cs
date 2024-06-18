using API.Interfaces;
using API.Models;
using API.Utility;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using static API.Utility.SD;
using API.Models.DTO.UserDTO.Requests;
using API.Models.DTO.UserDTO.Responses;
using System.Net;
using API.Helpers;

namespace API.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IEmailSender _emailSender;
        public AccountController(UserManager<ApplicationUser> userManager, IMapper mapper, IJwtTokenGenerator jwtTokenGenerator, IEmailSender emailSender)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwtTokenGenerator = jwtTokenGenerator;
            _emailSender = emailSender;
        }

        [HttpPost("Register")]

        public async Task<ActionResult> Register([FromBody] RegisterRequest registerDto, [FromServices] IValidator<RegisterRequest> validator)
        {
            ModelStateDictionary errors = ValidateModel.Validate(validator, registerDto);

            if (errors.Count > 0) {
                return ValidationProblem(errors);
            }

            var user = _mapper.Map<ApplicationUser>(registerDto);
            user.UserName = user.Email;
            user.FirstName = "User-" + Guid.NewGuid();
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, nameof(UserRoles.CUSTOMER));
            if (!roleResult.Succeeded)
                return BadRequest(result.Errors);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var emailMetadata = new EmailMetadata(user.Email, "Email verification", $"Confirmation token: {token}");
            await _emailSender.Send(emailMetadata);

            return Ok("Registration successfull");
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserResponse>> Login([FromBody] LoginRequest loginDto, [FromServices] IValidator<LoginRequest> validator)
        {
            ModelStateDictionary errors = ValidateModel.Validate(validator, loginDto);

            if (errors.Count > 0) {
                return ValidationProblem(errors);
            }

            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.NormalizedEmail == loginDto.Email.ToUpper());

            if (user == null)
                return BadRequest("Invalid Email or Password");

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return BadRequest("Invalid Email or Password");

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return BadRequest("Email verification required");


            return Ok(new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = await _jwtTokenGenerator.GenerateToken(user)
            });
        }

        [HttpPost("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail(VerifyEmailRequest model, IValidator<VerifyEmailRequest> validator)
        {
           ModelStateDictionary errors = ValidateModel.Validate(validator, model);

            if (errors.Count > 0) {
                return ValidationProblem(errors);
            }

            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.NormalizedEmail == model.Email.ToUpper());

            if (user == null)
                return BadRequest("User doesn't exist");

            var result = await _userManager.ConfirmEmailAsync(user, model.Token);

            if (!result.Succeeded)
                return BadRequest("Invalid token");

            return Ok("Email Verified");
        }

        [HttpPost("SendEmailToken")]
        public async Task<IActionResult> SendVerificationCode(EmailRequest email, IValidator<EmailRequest> validator)
        {
            ModelStateDictionary errors = ValidateModel.Validate(validator, email);

            if (errors.Count > 0) {
                return ValidationProblem(errors);
            }

            var user = await _userManager.FindByEmailAsync(email.Email);

            if (user == null)
                return BadRequest("User doesn't exist");

            if (await _userManager.IsEmailConfirmedAsync(user))
                return BadRequest("Email is already verified");

            var emailMetadata = new EmailMetadata(user.Email, "Password reset", await _userManager.GenerateEmailConfirmationTokenAsync(user));

            await _emailSender.Send(emailMetadata);

            return Ok();
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return BadRequest("The e-mail address is not assigned to any user account");

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return BadRequest("Email isn't verified");

            var templatePath = $"{Directory.GetCurrentDirectory()}/wwwroot/email/sample.cshtml";
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebUtility.UrlEncode(token);
            var baseUrl = $"https://localhost:4200/reset-password?email={user.Email}&token={encodedToken}";
            var emailMetadata = new EmailMetadata(user.Email, "Password Reset E-mail", baseUrl, templatePath);

            await _emailSender.Send(emailMetadata);

            return Ok();
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromQuery] string email, [FromQuery] string token, ResetPasswordRequest passwordDto, [FromServices] IValidator<ResetPasswordRequest> validator)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email is required");
            }

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is required");
            }

            ValidationResult validationResult = validator.Validate(passwordDto);

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

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return BadRequest("The e-mail address is not assigned to any user account");

            var result = await _userManager.ResetPasswordAsync(user, token, passwordDto.Password);

            if (!result.Succeeded)
                return BadRequest("Token is invalid");

            return Ok();
        }
    }
}
