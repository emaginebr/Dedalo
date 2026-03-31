using FluentValidation;
using System.Linq;

namespace Dedalo.Domain.Validators
{
    public static class ValidationHelper
    {
        public static void Validate<T>(IValidator<T> validator, T instance)
        {
            var result = validator.Validate(instance);
            if (!result.IsValid)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(result.Errors);
            }
        }
    }
}
