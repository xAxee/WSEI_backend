using AppCore.Entities;
using AppCore.Repository;

namespace Infrastructure.Memory;

public class MemoryParkingSessionRepository : MemoryGenericRepository<ParkingSession>, IParkingSessionRepositoryAsync
{
    public MemoryParkingSessionRepository()
    {
        var activeVehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            LicensePlate = "KR12345",
            Brand = "Toyota",
            Color = "Silver"
        };

        var historyVehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            LicensePlate = "WA98765",
            Brand = "Skoda",
            Color = "Black"
        };

        var activeSession = new ParkingSession
        {
            Id = Guid.NewGuid(),
            VehicleId = activeVehicle.Id,
            Vehicle = activeVehicle,
            GateName = "Brama Polnocna",
            EntryTime = new DateTime(2026, 4, 18, 8, 30, 0, DateTimeKind.Utc),
            ExitTime = null,
            ParkingFee = null,
            IsActive = true
        };

        var historicalSession = new ParkingSession
        {
            Id = Guid.NewGuid(),
            VehicleId = historyVehicle.Id,
            Vehicle = historyVehicle,
            GateName = "Brama Poludniowa",
            EntryTime = new DateTime(2026, 4, 17, 9, 0, 0, DateTimeKind.Utc),
            ExitTime = new DateTime(2026, 4, 17, 11, 45, 0, DateTimeKind.Utc),
            ParkingFee = 18.50m,
            IsActive = false
        };

        _data.Add(activeSession.Id, activeSession);
        _data.Add(historicalSession.Id, historicalSession);
    }

    public ParkingSession FindByNumberPlateAsync(string numberPlate)
    {
        return _data.Values
                   .Where(session => session.IsActive)
                   .FirstOrDefault(session =>
                       string.Equals(session.Vehicle.LicensePlate, numberPlate, StringComparison.OrdinalIgnoreCase))
               ?? throw new KeyNotFoundException($"Nie znaleziono aktywnej sesji dla pojazdu '{numberPlate}'.");
    }

    public List<ParkingSession> GetActiveSessionsAsync()
    {
        return _data.Values
            .Where(session => session.IsActive)
            .OrderByDescending(session => session.EntryTime)
            .ToList();
    }

    public List<ParkingSession> GetHistoryByNumberPlateAsync(string numberPlate)
    {
        return _data.Values
            .Where(session => !session.IsActive)
            .Where(session => string.Equals(session.Vehicle.LicensePlate, numberPlate, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(session => session.EntryTime)
            .ToList();
    }
}