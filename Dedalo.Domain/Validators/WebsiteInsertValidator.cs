using Dedalo.DTO.Website;
using FluentValidation;

namespace Dedalo.Domain.Validators
{
    public class WebsiteInsertValidator : AbstractValidator<WebsiteInsertInfo>
    {
        public WebsiteInsertValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(240).WithMessage("Name must be at most 240 characters");

            RuleFor(x => x.TemplateSlug)
                .MaximumLength(240).WithMessage("TemplateSlug must be at most 240 characters");

            RuleFor(x => x.DomainType)
                .IsInEnum().WithMessage("Invalid DomainType");

            RuleFor(x => x.CustomDomain)
                .MaximumLength(240).WithMessage("CustomDomain must be at most 240 characters");
        }
    }
}
