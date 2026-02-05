using residence.domain.Enums;

namespace residence.application.DTOs;

public record ResidentDto(
    Guid Id,
    Guid? UserId,
    Guid? HouseId,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Address,
    DateOnly? BirthDate,
    ResidentStatus Status,
    DateTime MoveInDate,
    DateTime? MoveOutDate,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);


