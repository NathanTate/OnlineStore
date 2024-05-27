using API.Models.DTO.ProductDTO.Requests;
using FluentValidation;

namespace API.FluentValidators.ProductValidators
{
    public class ProductRequestValidator : AbstractValidator<ProductRequest>
    {
        public ProductRequestValidator()
        {
            ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(p => p.Id).Empty();
            RuleFor(p => p.Name).NotEmpty().MaximumLength(150);
            RuleFor(p => p.Description).NotEmpty();
            RuleFor(p => p.SubCategoryId).NotEmpty();
            RuleFor(p => p.BrandId).NotEmpty();
            RuleFor(P => P.Colors).NotEmpty();
            RuleFor(p => p.ProductSpecifications).NotEmpty();
        }
    }
}
