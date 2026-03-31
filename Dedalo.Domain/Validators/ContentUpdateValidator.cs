using Dedalo.DTO.Content;
using FluentValidation;

namespace Dedalo.Domain.Validators
{
    public class ContentUpdateValidator : AbstractValidator<ContentUpdateInfo>
    {
        public ContentUpdateValidator()
        {
            RuleFor(x => x.ContentId)
                .GreaterThan(0).WithMessage("ContentId is required");

            RuleFor(x => x.ContentType)
                .NotEmpty().WithMessage("ContentType is required")
                .MaximumLength(100).WithMessage("ContentType must be at most 100 characters");

            RuleFor(x => x.ContentSlug)
                .MaximumLength(240).WithMessage("ContentSlug must be at most 240 characters");
        }
    }
}
