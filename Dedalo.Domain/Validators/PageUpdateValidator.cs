using Dedalo.DTO.Page;
using FluentValidation;

namespace Dedalo.Domain.Validators
{
    public class PageUpdateValidator : AbstractValidator<PageUpdateInfo>
    {
        public PageUpdateValidator()
        {
            RuleFor(x => x.PageId)
                .GreaterThan(0).WithMessage("PageId is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(240).WithMessage("Name must be at most 240 characters");

            RuleFor(x => x.PageSlug)
                .NotEmpty().WithMessage("PageSlug is required")
                .MaximumLength(240).WithMessage("PageSlug must be at most 240 characters");

            RuleFor(x => x.TemplatePageSlug)
                .MaximumLength(240).WithMessage("TemplatePageSlug must be at most 240 characters");
        }
    }
}
