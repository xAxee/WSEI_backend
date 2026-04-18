using AppCore.Entities;

namespace AppCore.Dto;

public record ParkingExitResultDto(
    Guid SessionId,
    VehicleDto Vehicle,
    string GateName,
    DateTime EntryTime,
    DateTime ExitTime,
    TimeSpan Duration,
    TimeSpan FreeParkingDuration,
    decimal Fee,
    bool WasCharged,
    string Message)
{
    public static ParkingExitResultDto FromEntity(
        ParkingSession session,
        TimeSpan freeParkingDuration,
        string message = "")
    {
        ArgumentNullException.ThrowIfNull(session);
        ArgumentNullException.ThrowIfNull(session.Vehicle);

        var resolvedExitTime = session.ExitTime
            ?? throw new InvalidOperationException("ParkingSession musi zawierać ExitTime do mapowania ParkingExitResultDto.");
        var resolvedFee = session.ParkingFee ?? 0m;

        return new ParkingExitResultDto(
            session.Id,
            VehicleDto.FromEntity(session.Vehicle),
            session.GateName,
            session.EntryTime,
            resolvedExitTime,
            resolvedExitTime - session.EntryTime,
            freeParkingDuration,
            resolvedFee,
            resolvedFee > 0,
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
            ExitTime = ExitTime,
            ParkingFee = Fee,
            IsActive = false
        };
    }
}
