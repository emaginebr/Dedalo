using Dedalo.DTO.Menu;
using FluentValidation;

namespace Dedalo.Domain.Validators
{
    public class MenuInsertValidator : AbstractValidator<MenuInsertInfo>
    {
        public MenuInsertValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(240).WithMessage("Name must be at most 240 characters");

            RuleFor(x => x.LinkType)
                .IsInEnum().WithMessage("Invalid LinkType");

            RuleFor(x => x.ExternalLink)
                .MaximumLength(500).WithMessage("ExternalLink must be at most 500 characters");
        }
    }
}
