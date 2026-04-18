using AppCore.Entities;

namespace AppCore.Dto;

public record CameraCaptureDto(
    string LicensePlate,
    string Brand,
    string Color,
    string GateName,
    string? ImagePath = null)
{
    public static CameraCaptureDto FromEntity(CameraCapture capture)
    {
        ArgumentNullException.ThrowIfNull(capture);

        return new CameraCaptureDto(
            capture.LicensePlate,
            capture.Detectedbrand,
            capture.DetectedColor,
            capture.GateName,
            string.IsNullOrWhiteSpace(capture.ImagePath) ? null : capture.ImagePath);
    }

    public CameraCapture ToEntity(
        Guid? id = null,
        DateTime? capturedAt = null,
        CaptureType type = CaptureType.Entry) => new()
    {
        Id = id ?? Guid.Empty,
        GateName = GateName,
        LicensePlate = LicensePlate,
        Detectedbrand = Brand,
        DetectedColor = Color,
        CapturedAt = capturedAt ?? DateTime.UtcNow,
        ImagePath = ImagePath ?? string.Empty,
        Type = type
    };
}