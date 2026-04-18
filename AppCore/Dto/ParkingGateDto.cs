using AppCore.Entities;

namespace AppCore.Dto;

public record ParkingGateDto(
    Guid Id,
    string Name,
    string Type,
    string Location,
    bool IsOperational)
{
    public static ParkingGateDto FromEntity(ParkingGate gate)
    {
        ArgumentNullException.ThrowIfNull(gate);

        return new ParkingGateDto(
            gate.Id,
            gate.Name,
            gate.Type.ToString(),
            gate.Location,
            gate.IsOperational);
    }

    public ParkingGate ToEntity() => new()
    {
        Id = Id,
        Name = Name,
        Type = ParseGateType(Type),
        Location = Location,
        IsOperational = IsOperational
    };

    private static GateType ParseGateType(string value)
    {
        if (Enum.TryParse<GateType>(value, true, out var gateType))
        {
            return gateType;
        }

        throw new ArgumentException($"Niepoprawny typ bramy: '{value}'.", nameof(value));
    }
}