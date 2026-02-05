using residence.application.DTOs;
using residence.application.Interfaces;
using residence.domain.Entities;
using residence.application.Repositories;

namespace residence.application.Services;

/// <summary>
/// Implementation of User service
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new Exception("User not found");

        return MapToDto(user);
    }

    public async Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto dto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new Exception("User not found");

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.PhoneNumber = dto.PhoneNumber;
        user.AvatarUrl = dto.AvatarUrl;
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);

        return MapToDto(user);
    }

    public async Task DeleteUserAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new Exception("User not found");

        await _userRepository.DeleteAsync(id);
    }

    public async Task<PagedResultDto<UserDto>> GetUsersByResidenceAsync(Guid residenceId, PaginationDto pagination)
    {
        var users = await _userRepository.GetByResidenceAsync(residenceId);
        
        var total = users.Count();
        var items = users
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(MapToDto)
            .ToList();

        var totalPages = (int)Math.Ceiling(total / (double)pagination.PageSize);

        return new PagedResultDto<UserDto>(items, total, pagination.PageNumber, pagination.PageSize, totalPages);
    }

    private UserDto MapToDto(User user)
    {
        return new UserDto(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            user.PhoneNumber,
            (UserRole)user.Role,
            user.AvatarUrl,
            user.CreatedAt,
            user.UpdatedAt
        );
    }
}
