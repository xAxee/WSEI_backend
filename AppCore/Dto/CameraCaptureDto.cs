using AppCore.Entities;

namespace AppCore.Dto;

public record CameraCaptureDto(
    Guid Id,
    Guid GateId,
    string LicensePlate,
    string Brand,
    string Color,
    string GateName,
    DateTime CapturedAt,
    string Type,
    string? ImagePath = null)
{
    public static CameraCaptureDto FromEntity(CameraCapture capture)
    {
        ArgumentNullException.ThrowIfNull(capture);

        return new CameraCaptureDto(
            capture.Id,
            capture.GateId,
            capture.LicensePlate,
            capture.Detectedbrand,
            capture.DetectedColor,
            capture.GateName,
            capture.CapturedAt,
            capture.Type.ToString(),
            string.IsNullOrWhiteSpace(capture.ImagePath) ? null : capture.ImagePath);
    }

    public CameraCapture ToEntity() => new()
    {
        Id = Id,
        GateId = GateId,
        GateName = GateName,
        LicensePlate = LicensePlate,
        Detectedbrand = Brand,
        DetectedColor = Color,
        CapturedAt = CapturedAt,
        ImagePath = ImagePath ?? string.Empty,
        Type = ParseCaptureType(Type)
    };

    private static CaptureType ParseCaptureType(string value)
    {
        if (Enum.TryParse<CaptureType>(value, true, out var captureType))
        {
            return captureType;
        }

        throw new ArgumentException($"Niepoprawny typ rejestracji: '{value}'.", nameof(value));
    }
}