using residence.domain.Enums;

namespace residence.application.DTOs;

/// <summary>
/// DTO for updating user profile information
/// Can include updating the associated resident profile
/// </summary>
public record UpdateUserDto(
    string FirstName,
    string LastName,
    string PhoneNumber,
    string? AvatarUrl = null,
    Guid? ResidentId = null  // Can update resident association
);

