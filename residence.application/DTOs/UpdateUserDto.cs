using residence.domain.Enums;

namespace residence.application.DTOs;

/// <summary>
/// DTO for updating user profile information
/// Can include updating the associated resident profile and user role
/// </summary>
public record UpdateUserDto(
    string FirstName,
    string LastName,
    string PhoneNumber,
    UserRole? Role = null,  // Can update user role (Admin/Resident)
    string? AvatarUrl = null,
    Guid? ResidentId = null  // Can update resident association
);

