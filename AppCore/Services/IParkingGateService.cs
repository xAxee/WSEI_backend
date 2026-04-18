using AppCore.Dto;
using AppCore.Repository;

namespace AppCore.Services;

public interface IParkingGateService
{
    Task<PagedResult<ParkingGateDto>> GetAllPaged(int page, int size);
    Task<ParkingGateDto?> GetById(Guid id);
    Task<ParkingGateDto?> GetByName(string name);
    Task<ParkingGateDto> Create(CreateGateDto dto);
    Task<ParkingGateDto?> Update(Guid id, UpdateGateDto dto);
    Task<ParkingGateDto?> ChangeOperationalStatus(Guid id, bool isOperational);
}