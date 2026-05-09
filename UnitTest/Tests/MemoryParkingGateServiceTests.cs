using AppCore.Dto;
using AppCore.Exceptions;
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
        var captures = new MemoryCameraCaptureRepository();
        var unitOfWork = new MemoryParkingUnitOfWork(vehicles, sessions, gates, captures);

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

    [Fact]
    public async Task AddCapture_ShouldAddCaptureToExistingGate()
    {
        var gate = (await _service.GetAllPaged(1, 10)).Items.First();

        var created = await _service.AddCapture(gate.Id, new CreateCameraCaptureDto(
            "KR12345",
            "Audi",
            "Black",
            "Entry",
            "captures/entry-1.jpg"));

        var captures = await _service.GetCaptures(gate.Id, 1, 10);

        Assert.Equal(gate.Id, created.GateId);
        Assert.Equal(1, captures.TotalCount);
        Assert.Single(captures.Items);
    }

    [Fact]
    public async Task AddCapture_ShouldThrow_WhenGateDoesNotExist()
    {
        await Assert.ThrowsAsync<GateNotFoundException>(() => _service.AddCapture(
            Guid.NewGuid(),
            new CreateCameraCaptureDto("KR12345", "Audi", "Black", "Entry", "captures/entry-1.jpg")));
    }

    [Fact]
    public async Task DeleteCapture_ShouldRemoveExistingCapture()
    {
        var gate = (await _service.GetAllPaged(1, 10)).Items.First();
        var created = await _service.AddCapture(gate.Id, new CreateCameraCaptureDto(
            "KR12345",
            "Audi",
            "Black",
            "Entry",
            "captures/entry-1.jpg"));

        await _service.DeleteCapture(gate.Id, created.Id);
        var captures = await _service.GetCaptures(gate.Id, 1, 10);

        Assert.Empty(captures.Items);
    }
}