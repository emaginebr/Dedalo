using Dedalo.DTO.Content;
using FluentValidation;

namespace Dedalo.Domain.Validators
{
    public class ContentAreaValidator : AbstractValidator<ContentAreaInfo>
    {
        public ContentAreaValidator()
        {
            RuleFor(x => x.ContentSlug)
                .NotEmpty().WithMessage("ContentSlug is required")
                .MaximumLength(240).WithMessage("ContentSlug must be at most 240 characters");

            RuleFor(x => x.Items)
                .NotNull().WithMessage("Items is required");

            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(x => x.ContentType)
                    .NotEmpty().WithMessage("ContentType is required")
                    .MaximumLength(100).WithMessage("ContentType must be at most 100 characters");
            });
        }
    }
}
