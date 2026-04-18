using AppCore.Dto;
using AppCore.Repository;
using AppCore.Services;
using Infrastructure.Memory;

namespace UnitTest.Tests;

public class MemoryParkingGateServiceTests
{
    private readonly IParkingGateService _service;

    public MemoryParkingGateServiceTests()
    {
        var vehicles = new MemoryVehicleRepository();
        var sessions = new MemoryParkingSessionRepository();
        var gates = new MemoryParkingGateRepository();
        var unitOfWork = new MemoryParkingUnitOfWork(vehicles, sessions, gates);

        _service = new MemoryParkingGateService(unitOfWork);
    }

    [Fact]
    public async Task GetAllPaged_ShouldReturnSeededGates()
    {
        var result = await _service.GetAllPaged(1, 10);

        Assert.Equal(2, result.TotalCount);
        Assert.Equal(2, result.Items.Count);
    }

    [Fact]
    public async Task GetByName_ShouldReturnMatchingGate()
    {
        var result = await _service.GetByName("Brama Polnocna");

        Assert.NotNull(result);
        Assert.Equal("Brama Polnocna", result!.Name);
    }

    [Fact]
    public async Task Create_ShouldAddNewGateAsNonOperational()
    {
        var created = await _service.Create(new CreateGateDto("Brama Wschodnia", "Entry", "Wjazd boczny"));

        Assert.Equal("Brama Wschodnia", created.Name);
        Assert.False(created.IsOperational);
    }

    [Fact]
    public async Task ChangeOperationalStatus_ShouldUpdateExistingGate()
    {
        var existing = (await _service.GetAllPaged(1, 10)).Items.First();

        var updated = await _service.ChangeOperationalStatus(existing.Id, false);

        Assert.NotNull(updated);
        Assert.False(updated!.IsOperational);
    }
}