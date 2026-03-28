using AppCore.Entities;

namespace AppCore.Repository;

public interface IParkingGateRepository : IGenericRepositoryAsync<ParkingGate>
{
    ParkingGate GetParkingGateByName(string name);
}