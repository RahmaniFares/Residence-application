using residence.domain.Enums;

namespace residence.application.DTOs;

public record AuthResponseDto(
    string AccessToken,
    string RefreshToken,
    UserDto User
);
