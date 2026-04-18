using AppCore.Entities;
using AppCore.Repository;

namespace Infrastructure.Memory;

public class MemoryVehicleRepository : MemoryGenericRepository<Vehicle>, IVehicleRepository
{
    public MemoryVehicleRepository()
    {
        var firstVehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            LicensePlate = "KR12345",
            Brand = "Toyota",
            Color = "Silver"
        };

        var secondVehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            LicensePlate = "WA98765",
            Brand = "Skoda",
            Color = "Black"
        };

        _data.Add(firstVehicle.Id, firstVehicle);
        _data.Add(secondVehicle.Id, secondVehicle);
    }

    public Vehicle FindByNumberPlate(string numberPlate)
    {
        return _data.Values.FirstOrDefault(v =>
                   string.Equals(v.LicensePlate, numberPlate, StringComparison.OrdinalIgnoreCase))
               ?? throw new KeyNotFoundException($"Nie znaleziono pojazdu o numerze rejestracyjnym '{numberPlate}'.");
    }
}