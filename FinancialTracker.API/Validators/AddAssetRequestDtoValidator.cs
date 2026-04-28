using FinancialTracker.API.DTOs;
using FluentValidation;

namespace FinancialTracker.API.Validators;

public class AddAssetRequestDtoValidator:AbstractValidator<AddAssetRequestDto>
{
    public AddAssetRequestDtoValidator()
    {
        RuleFor(x => x.Symbol)
            .NotEmpty().WithMessage("Symbol is required.")
            .MaximumLength(10).WithMessage("Symbol cannot exceed 10 characters.");

        RuleFor(x => x.AssetType)
            .IsInEnum().WithMessage("Invalid asset type.");


        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative.")
            .LessThanOrEqualTo(1000000).WithMessage("Quantity exceeds maximum allowed limit.");

        RuleFor(x => x.AverageCost)
            .NotNull()
            .GreaterThan(0)
            .When(x => x.Quantity > 0)
            .WithMessage("Average cost is required and must be greater than zero when quantity is greater than zero.");

        RuleFor(x => x.TargetPrice)
            .GreaterThan(0).When(x => x.TargetPrice.HasValue).WithMessage("Target price must be greater than zero.");
    }
}
