using residence.application.DTOs;

namespace residence.application.Interfaces;

/// <summary>
/// Authentication service interface
/// </summary>
public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
    Task<AuthResponseDto> RegisterAsync(CreateUserDto dto, Guid residenceId);
    Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto dto);
}
