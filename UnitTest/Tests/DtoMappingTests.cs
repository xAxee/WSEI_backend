using AppCore.Dto;
using AppCore.Entities;

namespace UnitTest.Tests;

public class DtoMappingTests
{
    [Fact]
    public void VehicleDto_Should_Map_Both_Ways()
    {
        var vehicle = CreateVehicle();

        var dto = VehicleDto.FromEntity(vehicle);
        var entity = dto.ToEntity();

        Assert.Equal(vehicle.Id, dto.Id);
        Assert.Equal(vehicle.LicensePlate, dto.LicensePlate);
        Assert.Equal(vehicle.Brand, dto.Brand);
        Assert.Equal(vehicle.Color, dto.Color);

        Assert.Equal(dto.Id, entity.Id);
        Assert.Equal(dto.LicensePlate, entity.LicensePlate);
        Assert.Equal(dto.Brand, entity.Brand);
        Assert.Equal(dto.Color, entity.Color);
    }

    [Fact]
    public void GateDtos_Should_Map_Both_Ways()
    {
        var gate = new ParkingGate
        {
            Id = Guid.NewGuid(),
            Name = "Gate A",
            Type = GateType.Both,
            Location = "North",
            IsOperational = true
        };

        var gateDto = ParkingGateDto.FromEntity(gate);
        var gateEntity = gateDto.ToEntity();

        Assert.Equal(gate.Id, gateDto.Id);
        Assert.Equal("Both", gateDto.Type);
        Assert.Equal(gate.Type, gateEntity.Type);
        Assert.Equal(gate.Location, gateEntity.Location);
        Assert.True(gateEntity.IsOperational);

        var createDto = CreateGateDto.FromEntity(gate);
        var createdEntity = createDto.ToEntity(id: gate.Id, isOperational: false);

        Assert.Equal(gate.Name, createDto.Name);
        Assert.Equal("Both", createDto.Type);
        Assert.Equal(gate.Id, createdEntity.Id);
        Assert.Equal(GateType.Both, createdEntity.Type);
        Assert.False(createdEntity.IsOperational);
    }

    [Fact]
    public void TariffDtos_Should_Map_Both_Ways()
    {
        var tariff = new ParkingTariff
        {
            Id = Guid.NewGuid(),
            Name = "Standard",
            FreeParkingDuration = TimeSpan.FromMinutes(30),
            HourlyRate = 8.5m,
            DailymaxRate = 60m,
            IsActive = true
        };

        var tariffDto = ParkingTariffDto.FromEntity(tariff);
        var tariffEntity = tariffDto.ToEntity();

        Assert.Equal(tariff.Id, tariffDto.Id);
        Assert.Equal(tariff.FreeParkingDuration, tariffDto.FreeParkingDuration);
        Assert.Equal(tariff.DailymaxRate, tariffDto.DailyMaxRate);
        Assert.Equal(tariff.DailymaxRate, tariffEntity.DailymaxRate);
        Assert.True(tariffEntity.IsActive);

        var createDto = CreateTariffDto.FromEntity(tariff);
        var createdEntity = createDto.ToEntity(id: tariff.Id, isActive: false);

        Assert.Equal(30, createDto.FreeMinutes);
        Assert.Equal(tariff.HourlyRate, createDto.HourlyRate);
        Assert.Equal(TimeSpan.FromMinutes(30), createdEntity.FreeParkingDuration);
        Assert.Equal(tariff.DailymaxRate, createdEntity.DailymaxRate);
        Assert.False(createdEntity.IsActive);
    }

    [Fact]
    public void CameraCaptureDto_Should_Map_Both_Ways()
    {
        var capturedAt = new DateTime(2026, 4, 18, 10, 15, 0, DateTimeKind.Utc);
        var capture = new CameraCapture
        {
            Id = Guid.NewGuid(),
            GateName = "Entry Gate",
            LicensePlate = "KR12345",
            Detectedbrand = "Audi",
            DetectedColor = "Black",
            CapturedAt = capturedAt,
            ImagePath = "captures/entry-1.jpg",
            Type = CaptureType.Entry
        };

        var dto = CameraCaptureDto.FromEntity(capture);
        var entity = dto.ToEntity(id: capture.Id, capturedAt: capture.CapturedAt, type: CaptureType.Exit);

        Assert.Equal(capture.LicensePlate, dto.LicensePlate);
        Assert.Equal(capture.Detectedbrand, dto.Brand);
        Assert.Equal(capture.DetectedColor, dto.Color);
        Assert.Equal(capture.ImagePath, dto.ImagePath);

        Assert.Equal(capture.Id, entity.Id);
        Assert.Equal(dto.Brand, entity.Detectedbrand);
        Assert.Equal(dto.Color, entity.DetectedColor);
        Assert.Equal(CaptureType.Exit, entity.Type);
    }

    [Fact]
    public void ActiveParkingAndEntryDtos_Should_Map_Session()
    {
        var entryTime = new DateTime(2026, 4, 18, 9, 0, 0, DateTimeKind.Utc);
        var now = entryTime.AddHours(2);
        var session = CreateSession(entryTime, null, null, true);

        var activeDto = ActiveParkingSessionDto.FromEntity(session, now);
        var activeEntity = activeDto.ToEntity();

        Assert.Equal(session.Id, activeDto.SessionId);
        Assert.Equal(TimeSpan.FromHours(2), activeDto.CurrentDuration);
        Assert.True(activeEntity.IsActive);
        Assert.Equal(session.Vehicle.Id, activeEntity.VehicleId);

        var entryDto = ParkingEntryResultDto.FromEntity(session, "Vehicle entered");
        var entryEntity = entryDto.ToEntity();

        Assert.Equal("Vehicle entered", entryDto.Message);
        Assert.Equal(session.Vehicle.LicensePlate, entryDto.Vehicle.LicensePlate);
        Assert.True(entryEntity.IsActive);
        Assert.Null(entryEntity.ExitTime);
    }

    [Fact]
    public void ExitAndHistoryDtos_Should_Map_Closed_Session()
    {
        var entryTime = new DateTime(2026, 4, 18, 8, 0, 0, DateTimeKind.Utc);
        var exitTime = entryTime.AddHours(3);
        var session = CreateSession(entryTime, exitTime, 25m, false);

        var exitDto = ParkingExitResultDto.FromEntity(session, TimeSpan.FromMinutes(30), "Vehicle exited");
        var exitEntity = exitDto.ToEntity();

        Assert.Equal(TimeSpan.FromHours(3), exitDto.Duration);
        Assert.Equal(25m, exitDto.Fee);
        Assert.True(exitDto.WasCharged);
        Assert.False(exitEntity.IsActive);
        Assert.Equal(exitTime, exitEntity.ExitTime);

        var historyDto = ParkingSessionHistoryDto.FromEntity(session);
        var historyEntity = historyDto.ToEntity();

        Assert.Equal(TimeSpan.FromHours(3), historyDto.Duration);
        Assert.Equal(25m, historyDto.Fee);
        Assert.False(historyEntity.IsActive);
        Assert.Equal(session.Vehicle.Id, historyEntity.VehicleId);
    }

    [Fact]
    public void ParkingStatsDto_Should_Aggregate_Sessions()
    {
        var today = new DateTime(2026, 4, 18, 12, 0, 0, DateTimeKind.Utc);
        var yesterday = today.AddDays(-1);

        var sessions = new List<ParkingSession>
        {
            CreateSession(today.AddHours(-2), null, null, true),
            CreateSession(today.AddHours(-5), today.AddHours(-1), 18m, false),
            CreateSession(yesterday.AddHours(-4), yesterday.AddHours(-1), 12m, false)
        };

        var dto = ParkingStatsDto.FromEntities(sessions, today);

        Assert.Equal(1, dto.ActiveVehicles);
        Assert.Equal(18m, dto.TodayRevenue);
        Assert.Equal(2, dto.TodayEntries);
        Assert.Equal(1, dto.TodayExits);
    }

    private static Vehicle CreateVehicle() => new()
    {
        Id = Guid.NewGuid(),
        LicensePlate = "KR12345",
        Brand = "Audi",
        Color = "Black"
    };

    private static ParkingSession CreateSession(DateTime entryTime, DateTime? exitTime, decimal? fee, bool isActive)
    {
        var vehicle = CreateVehicle();

        return new ParkingSession
        {
            Id = Guid.NewGuid(),
            VehicleId = vehicle.Id,
            Vehicle = vehicle,
            GateName = "Main Gate",
            EntryTime = entryTime,
            ExitTime = exitTime,
            ParkingFee = fee,
            IsActive = isActive
        };
    }
}