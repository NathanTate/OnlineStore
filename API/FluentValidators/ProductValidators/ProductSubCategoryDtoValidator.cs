using API.Models.DTO.ProductDTO;
using API.Models.DTO.ProductDTO.Requests;
using FluentValidation;

namespace API.FluentValidators.ProductValidators
{
    public class ProductSubCategoryDtoValidator: AbstractValidator<ProductSubCategoryDto>
    {
        public ProductSubCategoryDtoValidator()
        {
            ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(p => p.SubCategoryName).NotEmpty().MaximumLength(100);
            RuleFor(p => p.Image).NotEmpty().MaximumLength(500);
            RuleFor(p => p.SubCategoryDescription).NotEmpty();
            RuleFor(p => p.CategoryId).NotEmpty();
        }
    }
}
