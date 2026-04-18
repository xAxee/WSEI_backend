using AppCore.Entities;

namespace AppCore.Dto;

public record ParkingEntryResultDto(
    Guid SessionId,
    VehicleDto Vehicle,
    string GateName,
    DateTime EntryTime,
    string Message)
{
    public static ParkingEntryResultDto FromEntity(ParkingSession session, string message = "")
    {
        ArgumentNullException.ThrowIfNull(session);
        ArgumentNullException.ThrowIfNull(session.Vehicle);

        return new ParkingEntryResultDto(
            session.Id,
            VehicleDto.FromEntity(session.Vehicle),
            session.GateName,
            session.EntryTime,
            message);
    }

    public ParkingSession ToEntity()
    {
        var vehicleEntity = Vehicle.ToEntity();

        return new ParkingSession
        {
            Id = SessionId,
            VehicleId = vehicleEntity.Id,
            Vehicle = vehicleEntity,
            GateName = GateName,
            EntryTime = EntryTime,
            ExitTime = null,
            ParkingFee = null,
            IsActive = true
        };
    }
}