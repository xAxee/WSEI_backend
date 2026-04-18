using AppCore.Entities;

namespace AppCore.Dto;

public record CreateTariffDto(
    string Name,
    int FreeMinutes,
    decimal HourlyRate,
    decimal DailyMaxRate)
{
    public static CreateTariffDto FromEntity(ParkingTariff tariff)
    {
        ArgumentNullException.ThrowIfNull(tariff);

        return new CreateTariffDto(
            tariff.Name,
            (int)tariff.FreeParkingDuration.TotalMinutes,
            tariff.HourlyRate,
            tariff.DailymaxRate);
    }

    public ParkingTariff ToEntity(Guid? id = null, bool isActive = true) => new()
    {
        Id = id ?? Guid.Empty,
        Name = Name,
        FreeParkingDuration = TimeSpan.FromMinutes(FreeMinutes),
        HourlyRate = HourlyRate,
        DailymaxRate = DailyMaxRate,
        IsActive = isActive
    };
}