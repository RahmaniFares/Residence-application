using residence.application.DTOs;

namespace residence.application.Interfaces;

public interface IRappelService
{
    Task<RappelDto> CreateRappelAsync(Guid residenceId, CreateRappelDto dto);
    Task<RappelDto> GetRappelByIdAsync(Guid id);
    Task<RappelDto> UpdateRappelAsync(Guid id, UpdateRappelDto dto);
    Task DeleteRappelAsync(Guid id);
    Task<PagedResultDto<RappelDto>> GetRappelsByHouseAsync(Guid houseId, PaginationDto pagination);
    Task<PagedResultDto<RappelDto>> GetRappelsByResidenceAsync(Guid residenceId, PaginationDto pagination);
}
