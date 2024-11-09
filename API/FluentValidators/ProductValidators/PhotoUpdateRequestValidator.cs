using API.Models.DTO.ProductDTO.Requests;
using FluentValidation;

namespace API.FluentValidators.ProductValidators;

public class PhotoUpdateRequestValidator : AbstractValidator<PhotoUpdateRequest>
{
    public PhotoUpdateRequestValidator()
    {
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(p => p.ItemId).NotEmpty();
    }
}

