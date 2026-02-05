using residence.application.DTOs;

namespace residence.application.Interfaces;

/// <summary>
/// House service interface
/// </summary>
public interface IHouseService
{
    Task<HouseDto> CreateHouseAsync(Guid residenceId, CreateHouseDto dto);
    Task<HouseDto> GetHouseByIdAsync(Guid id);
    Task<HouseDetailDto> GetHouseDetailsAsync(Guid id);
    Task<HouseDto> UpdateHouseAsync(Guid id, UpdateHouseDto dto);
    Task DeleteHouseAsync(Guid id);
    Task<PagedResultDto<HouseDto>> GetHousesByResidenceAsync(Guid residenceId, PaginationDto pagination);
}
