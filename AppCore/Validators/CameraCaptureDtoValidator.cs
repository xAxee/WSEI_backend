using AppCore.Dto;
using FluentValidation;

namespace AppCore.Validators;

public class CameraCaptureDtoValidator : AbstractValidator<CameraCaptureDto>
{
    public CameraCaptureDtoValidator()
    {
        RuleFor(x => x.LicensePlate)
            .NotEmpty().WithMessage("Numer rejestracyjny jest wymagany.")
            .MaximumLength(10).WithMessage("Numer rejestracyjny nie może przekraczać 10 znaków.")
            .Matches(@"^[A-Z0-9]{2,10}$").WithMessage("Numer rejestracyjny ma niepoprawny format.");

        RuleFor(x => x.Brand)
            .NotEmpty().WithMessage("Marka pojazdu jest wymagana.")
            .MaximumLength(30).WithMessage("Marka pojazdu nie może przekraczać 30 znaków.")
            .Matches(@"^[\p{L}\d\s\-]+$").WithMessage("Marka pojazdu zawiera niedozwolone znaki.");

        RuleFor(x => x.Color)
            .NotEmpty().WithMessage("Kolor pojazdu jest wymagany.")
            .MaximumLength(20).WithMessage("Kolor pojazdu nie może przekraczać 20 znaków.")
            .Matches(@"^[\p{L}\d\s\-]+$").WithMessage("Kolor pojazdu zawiera niedozwolone znaki.");

        RuleFor(x => x.GateName)
            .NotEmpty().WithMessage("Nazwa bramki jest wymagana.")
            .MaximumLength(20).WithMessage("Nazwa bramki nie może przekraczać 20 znaków.")
            .Matches(@"^[\p{L}\d\s\-]+$").WithMessage("Nazwa bramki zawiera niedozwolone znaki.");

        RuleFor(x => x.ImagePath)
            .MaximumLength(260).WithMessage("Ścieżka obrazu nie może przekraczać 260 znaków.")
            .When(x => !string.IsNullOrWhiteSpace(x.ImagePath));
    }
}