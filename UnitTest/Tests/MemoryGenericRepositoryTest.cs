using AppCore.Entities;
using AppCore.Repository;
using Infrastructure.Memory;

public class MemoryGenericRepositoryTest
{
    private readonly IGenericRepositoryAsync<Vehicle> _repo = new MemoryGenericRepository<Vehicle>();

    [Fact]
    public async Task AddVehicleToRepositoryTestAsync()
    {
        var expected = new Vehicle()
        {
            LicensePlate = "TK 8434Y"
        };

        await _repo.AddAsync(expected);
        var actual = await _repo.FindByIdAsync(expected.Id);

        Assert.NotNull(actual);
        Assert.Equal(expected, actual);
        Assert.Equal(expected.Id, actual?.Id);
    }

    [Fact]
    public async Task FindAllAsync_ShouldReturnAllVehicles()
    {
        var repo = new MemoryGenericRepository<Vehicle>();

        var vehicle1 = new Vehicle { LicensePlate = "KR12345", Brand = "Audi", Color = "Black" };
        var vehicle2 = new Vehicle { LicensePlate = "WA54321", Brand = "BMW", Color = "White" };

        await repo.AddAsync(vehicle1);
        await repo.AddAsync(vehicle2);

        var result = await repo.FindAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, v => v.LicensePlate == "KR12345");
        Assert.Contains(result, v => v.LicensePlate == "WA54321");
    }

    [Fact]
    public async Task FindPagedAsync_ShouldReturnCorrectPage()
    {
        var repo = new MemoryGenericRepository<Vehicle>();

        for (int i = 1; i <= 5; i++)
        {
            await repo.AddAsync(new Vehicle
            {
                LicensePlate = $"KR{i}",
                Brand = "Test",
                Color = "Black"
            });
        }

        var result = await repo.FindPagedAsync(2, 2);

        Assert.NotNull(result);
        Assert.Equal(5, result.TotalCount);
        Assert.Equal(2, result.Page);
        Assert.Equal(2, result.PageSize);
        Assert.Equal(2, result.Items.Count());
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateExistingVehicle()
    {
        var repo = new MemoryGenericRepository<Vehicle>();

        var vehicle = new Vehicle
        {
            LicensePlate = "KR12345",
            Brand = "Audi",
            Color = "Black"
        };

        await repo.AddAsync(vehicle);

        vehicle.Brand = "BMW";
        vehicle.Color = "White";

        await repo.UpdateAsync(vehicle);
        var updated = await repo.FindByIdAsync(vehicle.Id);

        Assert.NotNull(updated);
        Assert.Equal("KR12345", updated.LicensePlate);
        Assert.Equal("BMW", updated.Brand);
        Assert.Equal("White", updated.Color);
    }

    [Fact]
    public async Task RemoveByIdAsync_ShouldRemoveVehicleFromRepository()
    {
        var repo = new MemoryGenericRepository<Vehicle>();

        var vehicle = new Vehicle
        {
            LicensePlate = "KR12345",
            Brand = "Audi",
            Color = "Black"
        };

        await repo.AddAsync(vehicle);

        await repo.RemoveByIdAsync(vehicle.Id);
        var result = await repo.FindByIdAsync(vehicle.Id);

        Assert.Null(result);
    }
}