using residence.application.DTOs;

namespace residence.application.Interfaces;

/// <summary>
/// Resident service interface
/// </summary>
public interface IResidentService
{
    Task<ResidentDto> CreateResidentAsync(Guid residenceId, CreateResidentDto dto);
    Task<ResidentDto> GetResidentByIdAsync(Guid id);
    Task<ResidentDto> UpdateResidentAsync(Guid id, UpdateResidentDto dto);
    Task DeleteResidentAsync(Guid id);
    Task<PagedResultDto<ResidentDto>> GetResidentsByResidenceAsync(Guid residenceId, PaginationDto pagination);
    Task<PagedResultDto<ResidentDto>> GetResidentsByHouseAsync(Guid houseId, PaginationDto pagination);
}
