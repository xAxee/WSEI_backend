using AppCore.Entities;

namespace AppCore.Dto;

public record ParkingSessionHistoryDto(
    Guid SessionId,
    VehicleDto Vehicle,
    string GateName,
    DateTime EntryTime,
    DateTime? ExitTime,
    TimeSpan? Duration,
    decimal? Fee,
    bool IsActive)
{
    public static ParkingSessionHistoryDto FromEntity(ParkingSession session)
    {
        ArgumentNullException.ThrowIfNull(session);
        ArgumentNullException.ThrowIfNull(session.Vehicle);

        TimeSpan? duration = session.ExitTime is null
            ? null
            : session.ExitTime.Value - session.EntryTime;

        return new ParkingSessionHistoryDto(
            session.Id,
            VehicleDto.FromEntity(session.Vehicle),
            session.GateName,
            session.EntryTime,
            session.ExitTime,
            duration,
            session.ParkingFee,
            session.IsActive);
    }

    public ParkingSession ToEntity()
    {
        var vehicleEntity = Vehicle.ToEntity();
        var resolvedExitTime = ExitTime ?? (Duration.HasValue && !IsActive ? EntryTime + Duration.Value : null);

        return new ParkingSession
        {
            Id = SessionId,
            VehicleId = vehicleEntity.Id,
            Vehicle = vehicleEntity,
            GateName = GateName,
            EntryTime = EntryTime,
            ExitTime = resolvedExitTime,
            ParkingFee = Fee,
            IsActive = IsActive
        };
    }
}