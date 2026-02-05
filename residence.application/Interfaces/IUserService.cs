using residence.application.DTOs;

namespace residence.application.Interfaces;

/// <summary>
/// User service interface
/// </summary>
public interface IUserService
{
    Task<UserDto> GetUserByIdAsync(Guid id);
    Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto dto);
    Task DeleteUserAsync(Guid id);
    Task<PagedResultDto<UserDto>> GetUsersByResidenceAsync(Guid residenceId, PaginationDto pagination);
}
