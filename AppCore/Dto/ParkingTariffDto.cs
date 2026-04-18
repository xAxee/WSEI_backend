using AppCore.Entities;

namespace AppCore.Dto;

public record ParkingTariffDto(
    Guid Id,
    string Name,
    TimeSpan FreeParkingDuration,
    decimal HourlyRate,
    decimal DailyMaxRate,
    bool IsActive)
{
    public static ParkingTariffDto FromEntity(ParkingTariff tariff)
    {
        ArgumentNullException.ThrowIfNull(tariff);

        return new ParkingTariffDto(
            tariff.Id,
            tariff.Name,
            tariff.FreeParkingDuration,
            tariff.HourlyRate,
            tariff.DailymaxRate,
            tariff.IsActive);
    }

    public ParkingTariff ToEntity() => new()
    {
        Id = Id,
        Name = Name,
        FreeParkingDuration = FreeParkingDuration,
        HourlyRate = HourlyRate,
        DailymaxRate = DailyMaxRate,
        IsActive = IsActive
    };
}