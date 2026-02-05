using residence.domain.Enums;

namespace residence.application.DTOs;

public record UserDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    UserRole Role,
    string? AvatarUrl,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

