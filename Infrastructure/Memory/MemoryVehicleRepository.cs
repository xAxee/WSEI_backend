using AppCore.Entities;
using AppCore.Repository;

namespace Infrastructure.Memory;

public class MemoryVehicleRepository : MemoryGenericRepository<Vehicle>, IVehicleRepository
{
    public Vehicle FindByNumberPlate(string numberPlate)
    {
        var vehicle = _data.Values.FirstOrDefault(v =>
            string.Equals(v.LicensePlate, numberPlate, StringComparison.OrdinalIgnoreCase));
        
        return vehicle;
    }
}