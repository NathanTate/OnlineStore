using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Helpers
{
  public static class ValidateModel
  {
    public static ModelStateDictionary Validate<T>(IValidator<T> validator, T model)
    {
      ValidationResult validationResult = validator.Validate(model);
      var modelStateDictionary = new ModelStateDictionary();

      if (!validationResult.IsValid)
      {
        foreach (ValidationFailure failure in validationResult.Errors)
        {
          modelStateDictionary.AddModelError(
              failure.PropertyName,
              failure.ErrorMessage);
        }
      }

      return modelStateDictionary;
    }
  }
}