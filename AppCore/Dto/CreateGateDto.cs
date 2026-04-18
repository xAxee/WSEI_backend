using AppCore.Entities;

namespace AppCore.Dto;

public record CreateGateDto(
    string Name,
    string Type,
    string Location)
{
    public static CreateGateDto FromEntity(ParkingGate gate)
    {
        ArgumentNullException.ThrowIfNull(gate);

        return new CreateGateDto(
            gate.Name,
            gate.Type.ToString(),
            gate.Location);
    }

    public ParkingGate ToEntity(Guid? id = null, bool isOperational = true) => new()
    {
        Id = id ?? Guid.Empty,
        Name = Name,
        Type = ParseGateType(Type),
        Location = Location,
        IsOperational = isOperational
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