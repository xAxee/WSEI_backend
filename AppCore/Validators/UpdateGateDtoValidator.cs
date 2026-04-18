using AppCore.Dto;
using AppCore.Entities;
using FluentValidation;

namespace AppCore.Validators;

public class UpdateGateDtoValidator : AbstractValidator<UpdateGateDto>
{
    public UpdateGateDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nazwa bramki jest wymagana.")
            .MaximumLength(20).WithMessage("Nazwa nie może przekraczać 20 znaków.")
            .Matches(@"^[\p{L}\d\s\-]+$").WithMessage("Nazwa zawiera niedozwolone znaki.");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Typ bramki jest wymagany.")
            .Must(type => Enum.TryParse<GateType>(type, true, out _))
            .WithMessage("Typ bramki musi być jedną z wartości wyliczenia GateType.");
    }
}