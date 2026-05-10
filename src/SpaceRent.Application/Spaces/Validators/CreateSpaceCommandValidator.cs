using FluentValidation;
using SpaceRent.Application.Spaces.Commands;

namespace SpaceRent.Application.Spaces.Validators;

public class CreateSpaceCommandValidator : AbstractValidator<CreateSpaceCommand>
{
    public CreateSpaceCommandValidator()
    {
        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(v => v.PricePerHour)
            .GreaterThan(0).WithMessage("Price per hour must be greater than zero.");

        RuleFor(v => v.Location)
            .NotEmpty().WithMessage("Location is required.");

        RuleFor(v => v.OwnerId)
            .NotEmpty().WithMessage("OwnerId is required.");
    }
}
