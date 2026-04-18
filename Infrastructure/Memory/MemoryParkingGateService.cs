using AppCore.Dto;
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
}