using AppCore.Repository;

namespace Infrastructure.Memory;

public class MemoryParkingUnitOfWork(
    IVehicleRepository vehicles,
    IParkingSessionRepositoryAsync sessions,
    IParkingGateRepository gates) : IParkingUnitOfWork
{
    public IVehicleRepository Vehicles => vehicles;
    public IParkingGateRepository Gates => gates;
    public IParkingSessionRepositoryAsync Sessions => sessions;

    public Task<int> SaveChangesAsync()
    {
        return Task.FromResult(0);
    }

    public Task BeginTransactionAsync()
    {
        return Task.CompletedTask;
    }

    public Task CommitTransactionAsync()
    {
        return Task.CompletedTask;
    }

    public Task RollbackTransactionAsync()
    {
        return Task.CompletedTask;
    }
}