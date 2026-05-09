using AppCore.Entities;
using AppCore.Repository;

namespace Infrastructure.Memory;

public class MemoryParkingGateRepository : MemoryGenericRepository<ParkingGate>, IParkingGateRepository
{
    public MemoryParkingGateRepository()
    {
        var firstGate = new ParkingGate
        {
            Id = Guid.Parse("3d54091d-abc8-49ec-9590-93ad3ed5458f"),
            Name = "Brama Polnocna",
            Type = GateType.Entry,
            Location = "Wjazd od ul. Glownej",
            IsOperational = true
        };

        var secondGate = new ParkingGate
        {
            Id = Guid.Parse("0349e001-57d0-4abf-86ea-c9575ea4ff34"),
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