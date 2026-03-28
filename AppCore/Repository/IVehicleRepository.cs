using AppCore.Entities;

namespace AppCore.Repository;

public interface IVehicleRepository : IGenericRepositoryAsync<Vehicle>
{
    Vehicle FindByNumberPlate(string numberPlate);
}