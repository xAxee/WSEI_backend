using AppCore.Entities;

namespace AppCore.Dto;

public record ParkingStatsDto(
    int ActiveVehicles,
    decimal TodayRevenue,
    int TodayEntries,
    int TodayExits)
{
    public static ParkingStatsDto FromEntities(IEnumerable<ParkingSession> sessions, DateTime? referenceDate = null)
    {
        ArgumentNullException.ThrowIfNull(sessions);

        var sessionList = sessions.ToList();
        var day = (referenceDate ?? DateTime.UtcNow).Date;

        return new ParkingStatsDto(
            sessionList.Count(session => session.IsActive),
            sessionList
                .Where(session => session.ExitTime.HasValue && session.ExitTime.Value.Date == day)
                .Sum(session => session.ParkingFee ?? 0m),
            sessionList.Count(session => session.EntryTime.Date == day),
            sessionList.Count(session => session.ExitTime.HasValue && session.ExitTime.Value.Date == day));
    }
}