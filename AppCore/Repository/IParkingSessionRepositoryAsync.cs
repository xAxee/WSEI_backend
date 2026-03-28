using AppCore.Entities;

namespace AppCore.Repository;

public interface IParkingSessionRepositoryAsync : IGenericRepositoryAsync<ParkingSession>
{
    ParkingSession FindByNumberPlateAsync(string numberPlate);
    List<ParkingSession> GetActiveSessionsAsync();
    List<ParkingSession> GetHistoryByNumberPlateAsync(string numberPlate);
}