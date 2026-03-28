namespace AppCore.Dto;

public record CreateGateDto(
    string Name,
    string Type,
    string Location
);