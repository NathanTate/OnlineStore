using API.Models.DTO.ProductDTO.Requests;
using FluentValidation;

namespace API.FluentValidators.ProductValidators
{
    public class ProductCategoryRequestValidator : AbstractValidator<ProductCategoryResponse>
    {
        public ProductCategoryRequestValidator()
        {
            ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(p => p.CategoryName).NotEmpty().MaximumLength(100);
            RuleFor(p => p.CategoryDescription).NotEmpty();
        }
    }
}
