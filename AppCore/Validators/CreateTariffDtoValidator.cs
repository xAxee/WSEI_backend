using AppCore.Dto;
using FluentValidation;

namespace AppCore.Validators;

public class CreateTariffDtoValidator : AbstractValidator<CreateTariffDto>
{
    public CreateTariffDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nazwa taryfy jest wymagana.")
            .MaximumLength(50).WithMessage("Nazwa taryfy nie może przekraczać 50 znaków.")
            .Matches(@"^[\p{L}\d\s\-]+$").WithMessage("Nazwa taryfy zawiera niedozwolone znaki.");

        RuleFor(x => x.FreeMinutes)
            .GreaterThanOrEqualTo(0).WithMessage("Liczba darmowych minut nie może być ujemna.");

        RuleFor(x => x.HourlyRate)
            .GreaterThanOrEqualTo(0).WithMessage("Stawka godzinowa nie może być ujemna.");

        RuleFor(x => x.DailyMaxRate)
            .GreaterThanOrEqualTo(0).WithMessage("Maksymalna stawka dobowa nie może być ujemna.")
            .GreaterThanOrEqualTo(x => x.HourlyRate)
            .WithMessage("Maksymalna stawka dobowa nie może być mniejsza od stawki godzinowej.");
    }
}