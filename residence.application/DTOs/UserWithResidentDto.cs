using residence.domain.Enums;

namespace residence.application.DTOs;

/// <summary>
/// User DTO with associated resident information
/// Used when retrieving user details including their resident profile
/// </summary>
public record UserWithResidentDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    UserRole Role,
    string? AvatarUrl,
    ResidentDto? Resident,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
