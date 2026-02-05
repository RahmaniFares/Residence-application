using residence.application.DTOs;
using residence.application.Interfaces;
using residence.domain.Entities;
using residence.application.Repositories;

namespace residence.application.Services;

/// <summary>
/// Implementation of Authentication service
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null)
            throw new Exception("Invalid email or password");

        // Verify password (simplified - use BCrypt in production)
        if (!VerifyPassword(dto.Password, user.PasswordHash))
            throw new Exception("Invalid email or password");

        var tokens = GenerateTokens(user);
        var userDto = MapToUserDto(user);

        return new AuthResponseDto(tokens.AccessToken, tokens.RefreshToken, userDto);
    }

    public async Task<AuthResponseDto> RegisterAsync(CreateUserDto dto, Guid residenceId)
    {
        var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
        if (existingUser != null)
            throw new Exception("Email already registered");

        var user = new User
        {
            Id = Guid.NewGuid(),
            ResidenceId = residenceId,
            Email = dto.Email,
            PasswordHash = HashPassword(dto.Password),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.PhoneNumber,
            Role = (domain.Enums.UserRole)(int)dto.Role,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _userRepository.AddAsync(user);
        
        var tokens = GenerateTokens(created);
        var userDto = MapToUserDto(created);

        return new AuthResponseDto(tokens.AccessToken, tokens.RefreshToken, userDto);
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto dto)
    {
        // TODO: Implement JWT refresh token validation
        // For now, return a placeholder
        throw new NotImplementedException("Refresh token implementation pending");
    }

    private bool VerifyPassword(string password, string hash)
    {
        // TODO: Use BCrypt.Net-Next or similar
        // Simplified for now - replace with proper password verification
        return hash == HashPassword(password);
    }

    private string HashPassword(string password)
    {
        // TODO: Use BCrypt.Net-Next or similar
        // Simplified for now - replace with proper password hashing
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
    }

    private (string AccessToken, string RefreshToken) GenerateTokens(User user)
    {
        // TODO: Implement JWT token generation
        // For now, return placeholder tokens
        var accessToken = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{user.Id}:{user.Email}"));
        var refreshToken = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));

        return (accessToken, refreshToken);
    }

    private UserDto MapToUserDto(User user)
    {
        return new UserDto(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            user.PhoneNumber,
            (residence.application.DTOs.UserRole)user.Role,
            user.AvatarUrl,
            user.CreatedAt,
            user.UpdatedAt
        );
    }
}
