using residence.domain.Enums;

namespace residence.application.DTOs;

public record UpdateUserDto(
    string FirstName,
    string LastName,
    string PhoneNumber,
    string? AvatarUrl = null
);

