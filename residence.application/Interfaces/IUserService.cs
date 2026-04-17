using residence.application.DTOs;

namespace residence.application.Interfaces;

/// <summary>
/// User service interface
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Create a new user in a residence
    /// </summary>
    Task<UserDto> CreateUserAsync(Guid residenceId, CreateUserDto dto);

    Task<UserDto> GetUserByIdAsync(Guid id);
    Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto dto);
    Task DeleteUserAsync(Guid id);
    Task<PagedResultDto<UserDto>> GetUsersByResidenceAsync(Guid residenceId, PaginationDto pagination);
}
