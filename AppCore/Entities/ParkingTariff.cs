namespace AppCore.Entities;

public class ParkingTariff : EntityBase
{
    public string Name { get; set; }
    public TimeSpan FreeParkingDuration { get; set; }
    public decimal HourlyRate { get; set; }
    public decimal DailymaxRate { get; set; }
    public bool IsActive { get; set; }
}