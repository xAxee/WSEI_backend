using AppCore.Entities;

namespace AppCore.Dto;

public record UpdateGateDto(
    string Name,
    string Type)
{
    public GateType ToGateType()
    {
        if (Enum.TryParse<GateType>(Type, true, out var gateType))
        {
            return gateType;
        }

        throw new ArgumentException($"Niepoprawny typ bramy: '{Type}'.", nameof(Type));
    }
}