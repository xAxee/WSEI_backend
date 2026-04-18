using AppCore.Entities;

namespace AppCore.Dto;

public record ActiveParkingSessionDto(
    Guid SessionId,
    VehicleDto Vehicle,
    string GateName,
    DateTime EntryTime,
    TimeSpan CurrentDuration)
{
    public static ActiveParkingSessionDto FromEntity(ParkingSession session, DateTime? currentTime = null)
    {
        ArgumentNullException.ThrowIfNull(session);
        ArgumentNullException.ThrowIfNull(session.Vehicle);

        var resolvedCurrentTime = currentTime ?? DateTime.UtcNow;
        var endTime = session.ExitTime ?? resolvedCurrentTime;

        return new ActiveParkingSessionDto(
            session.Id,
            VehicleDto.FromEntity(session.Vehicle),
            session.GateName,
            session.EntryTime,
            endTime - session.EntryTime);
    }

    public ParkingSession ToEntity(bool isActive = true)
    {
        var vehicleEntity = Vehicle.ToEntity();

        return new ParkingSession
        {
            Id = SessionId,
            VehicleId = vehicleEntity.Id,
            Vehicle = vehicleEntity,
            GateName = GateName,
            EntryTime = EntryTime,
            ExitTime = isActive ? null : EntryTime + CurrentDuration,
            ParkingFee = null,
            IsActive = isActive
        };
    }
}