using API.Models.DTO.ProductDTO.Requests;
using FluentValidation;

namespace API.FluentValidators.ProductValidators;

public class SetMainPhotoRequestValidator : AbstractValidator<SetMainPhotoRequest>
{
    public SetMainPhotoRequestValidator()
    {
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(p => p.itemId).NotEmpty().GreaterThan(0);
        RuleFor(p => p.photoId).NotEmpty().GreaterThan(0);
    }
}

