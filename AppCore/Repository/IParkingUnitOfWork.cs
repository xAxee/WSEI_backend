namespace AppCore.Repository;

public interface IParkingUnitOfWork
{
    IVehicleRepository Vehicles { get; }
    IParkingGateRepository Gates { get; }
    IParkingSessionRepositoryAsync Sessions { get; }

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}