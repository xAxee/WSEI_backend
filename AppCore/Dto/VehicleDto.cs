using AppCore.Entities;

namespace AppCore.Dto;

public record VehicleDto(
    Guid Id,
    string LicensePlate,
    string Brand,
    string Color)
{
    public static VehicleDto FromEntity(Vehicle vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle);

        return new VehicleDto(
            vehicle.Id,
            vehicle.LicensePlate,
            vehicle.Brand,
            vehicle.Color);
    }

    public Vehicle ToEntity() => new()
    {
        Id = Id,
        LicensePlate = LicensePlate,
        Brand = Brand,
        Color = Color
    };
}