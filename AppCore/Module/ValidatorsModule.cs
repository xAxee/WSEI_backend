using AppCore.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.Module;

public static class ValidatorsModule
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<ParkingGateValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateTariffDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<CameraCaptureDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateGateDtoValidator>();
        
        services.AddFluentValidationAutoValidation();

        return services;
    }
}