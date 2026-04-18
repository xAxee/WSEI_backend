using AppCore.Entities;
using AppCore.Repository;

namespace Infrastructure.Memory;

public class MemoryParkingGateRepository : MemoryGenericRepository<ParkingGate>, IParkingGateRepository
{
    public MemoryParkingGateRepository()
    {
        var firstGate = new ParkingGate
        {
            Id = Guid.NewGuid(),
            Name = "Brama Polnocna",
            Type = GateType.Entry,
            Location = "Wjazd od ul. Glownej",
            IsOperational = true
        };

        var secondGate = new ParkingGate
        {
            Id = Guid.NewGuid(),
            Name = "Brama Poludniowa",
            Type = GateType.Exit,
            Location = "Wyjazd od ul. Parkowej",
            IsOperational = true
        };

        _data.Add(firstGate.Id, firstGate);
        _data.Add(secondGate.Id, secondGate);
    }

    public Task<ParkingGate?> FindByNameAsync(string name)
    {
        var gate = _data.Values.FirstOrDefault(g =>
            string.Equals(g.Name, name, StringComparison.OrdinalIgnoreCase));

        return Task.FromResult(gate);
    }
}