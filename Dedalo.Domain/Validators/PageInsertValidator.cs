using Dedalo.DTO.Page;
using FluentValidation;

namespace Dedalo.Domain.Validators
{
    public class PageInsertValidator : AbstractValidator<PageInsertInfo>
    {
        public PageInsertValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(240).WithMessage("Name must be at most 240 characters");

            RuleFor(x => x.TemplatePageSlug)
                .MaximumLength(240).WithMessage("TemplatePageSlug must be at most 240 characters");
        }
    }
}
