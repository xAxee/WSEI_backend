using AppCore.Dto;
using AppCore.Exceptions;
using AppCore.Repository;
using AppCore.Services;

namespace Infrastructure.Memory;

public class MemoryParkingGateService(IParkingUnitOfWork unit) : IParkingGateService
{
    public async Task<PagedResult<ParkingGateDto>> GetAllPaged(int page, int size)
    {
        var pagedResult = await unit.Gates.FindPagedAsync(page, size);

        return new PagedResult<ParkingGateDto>(
            pagedResult.Items.Select(ParkingGateDto.FromEntity).ToList(),
            pagedResult.TotalCount,
            pagedResult.Page,
            pagedResult.PageSize);
    }

    public async Task<ParkingGateDto?> GetById(Guid id)
    {
        var entity = await unit.Gates.FindByIdAsync(id);
        return entity is null ? null : ParkingGateDto.FromEntity(entity);
    }

    public async Task<ParkingGateDto?> GetByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        var entity = await unit.Gates.FindByNameAsync(name);
        return entity is null ? null : ParkingGateDto.FromEntity(entity);
    }

    public async Task<ParkingGateDto> Create(CreateGateDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var entity = dto.ToEntity(isOperational: false);
        var created = await unit.Gates.AddAsync(entity);
        await unit.SaveChangesAsync();

        return ParkingGateDto.FromEntity(created);
    }

    public async Task<ParkingGateDto?> Update(Guid id, UpdateGateDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var entity = await unit.Gates.FindByIdAsync(id);
        if (entity is null)
        {
            return null;
        }

        entity.Name = dto.Name;
        entity.Type = dto.ToGateType();

        await unit.Gates.UpdateAsync(entity);
        await unit.SaveChangesAsync();

        return ParkingGateDto.FromEntity(entity);
    }

    public async Task<ParkingGateDto?> ChangeOperationalStatus(Guid id, bool isOperational)
    {
        var entity = await unit.Gates.FindByIdAsync(id);
        if (entity is null)
        {
            return null;
        }

        entity.IsOperational = isOperational;

        await unit.Gates.UpdateAsync(entity);
        await unit.SaveChangesAsync();

        return ParkingGateDto.FromEntity(entity);
    }

    public async Task<CameraCaptureDto> AddCapture(Guid gateId, CreateCameraCaptureDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var gate = await unit.Gates.FindByIdAsync(gateId)
                   ?? throw new GateNotFoundException($"Gate with id={gateId} not found!");

        var entity = dto.ToEntity(gateId, gate.Name);
        var created = await unit.Captures.AddAsync(entity);
        gate.CameraCaptures.Add(created);

        await unit.Gates.UpdateAsync(gate);
        await unit.SaveChangesAsync();

        return CameraCaptureDto.FromEntity(created);
    }

    public async Task<PagedResult<CameraCaptureDto>> GetCaptures(Guid gateId, int page, int size)
    {
        var gate = await unit.Gates.FindByIdAsync(gateId)
                   ?? throw new GateNotFoundException($"Gate with id={gateId} not found!");

        var pagedResult = await unit.Captures.FindPagedByGateIdAsync(gate.Id, page, size);

        return new PagedResult<CameraCaptureDto>(
            pagedResult.Items.Select(CameraCaptureDto.FromEntity).ToList(),
            pagedResult.TotalCount,
            pagedResult.Page,
            pagedResult.PageSize);
    }

    public async Task DeleteCapture(Guid gateId, Guid captureId)
    {
        var gate = await unit.Gates.FindByIdAsync(gateId)
                   ?? throw new GateNotFoundException($"Gate with id={gateId} not found!");

        var capture = await unit.Captures.FindByGateIdAndCaptureIdAsync(gateId, captureId)
                      ?? throw new CaptureNotFoundException($"Capture with id={captureId} for gate id={gateId} not found!");

        await unit.Captures.RemoveByIdAsync(capture.Id);
        gate.CameraCaptures.RemoveAll(existing => existing.Id == capture.Id);

        await unit.Gates.UpdateAsync(gate);
        await unit.SaveChangesAsync();
    }
}